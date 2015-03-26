using InfoQ.Viewer.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AssetDownloader
{
    public class InfoQDownloader : IAssetDownloader
    {
        IMediaRepositoryFactory repoFacotry;
        IAssetRequester assetRequester;
        IContentParser contentParser;
        IProgress<string> progress;

        IMediaRepository repo;  //initialize though download method

        public InfoQDownloader(IMediaRepositoryFactory repofactory, IAssetRequester requester, IContentParser parser, IProgress<string> progress)
        {
            if (repofactory == null)
                throw new ArgumentNullException("Downloader requires a Repository instance.");
            if (requester == null)
                throw new ArgumentNullException("Downloader requires a Requester instance.");
            if (parser == null)
                throw new ArgumentNullException("Downloader requires a Parser instance.");
            if (progress == null)
                throw new ArgumentNullException("Downloader requires a Progress instance.");

            repoFacotry = repofactory;
            assetRequester = requester;
            contentParser = parser;
            this.progress = progress;
        }

        public async Task<bool> DownloadPresentation(string presentationAddress)
        {
            if(string.IsNullOrWhiteSpace(presentationAddress))
                throw new ArgumentNullException("A valid address must to supplied");

            //1- get Html
            //2- parse html, get metadata

            progress.Report("getting content...");
            string html = assetRequester.GetHtmlContent(presentationAddress);
            IPresentationAssetsMetaData webMetadata = contentParser.GetAssetsMetadata(html);
            progress.Report("parsing done...");

            //3 create repo
            progress.Report("creating repo for " + webMetadata.FriendlyName + "...");
            this.repo = repoFacotry.Create(webMetadata.FriendlyName);
            repo.CreateRepositoryForPresentation();

            //5 get and save assets
            progress.Report("starting downloads...");
            IPresentationAssetsMetaData localData = await DownloadAssetsToRepository(webMetadata);

            //save metadata
            progress.Report("saving metadata...");
            return repo.SaveMetadataFile(localData);

            //register?

        }

        private async Task<IPresentationAssetsMetaData> DownloadAssetsToRepository(IPresentationAssetsMetaData webMetadata)
        {
            IPresentationAssetsMetaData newMetaData =  CopyBaseMetadataToLocalMetadataObject(webMetadata);

            List<Task> downloadTasks = new List<Task>();

            //TODO: Uncomment video!
            downloadTasks.Add(DownloadVideoFile(webMetadata, newMetaData)); //longest - start first
            //download slide images
            downloadTasks.AddRange(DownloadSlideImages(webMetadata, newMetaData));
            downloadTasks.Add(DownloadThumbImage(webMetadata, newMetaData));
            
            //todo download mp3, pdf
            await Task.WhenAll(downloadTasks);

            return newMetaData;
        }

        private Task DownloadVideoFile(IPresentationAssetsMetaData webMetadata, IPresentationAssetsMetaData newMetaData)
        {
            //TODO: rethink this for large file!
            string filename = string.Format("{0}.mp4", webMetadata.FriendlyName);
            newMetaData.VideoFileAddress = filename;
            
            //go async!
            return assetRequester.AsyncAssetGet(webMetadata.VideoFileAddress, 
                                                repo.GetPathForAsset(AssetType.Video, filename),
                                                (s, e) => { progress.Report(string.Format("video {0} % complete ...", e.ProgressPercentage)); },
                                                (s, e) => { progress.Report("video download complete..."); });            
        }

        private Task DownloadThumbImage(IPresentationAssetsMetaData webMetadata, IPresentationAssetsMetaData newMetaData)
        {
            newMetaData.ThumbnailImageAddress = "thumb.jpg";
            
            //go async!
            return assetRequester.AsyncAssetGet(
                            webMetadata.ThumbnailImageAddress,
                            repo.GetPathForAsset(AssetType.ThumbnailImage, "thumb.jpg"),
                            null,
                            (s, e) => { progress.Report("thumbnail download complete..."); });
        }

        private List<Task> DownloadSlideImages(IPresentationAssetsMetaData webMetadata, IPresentationAssetsMetaData newMetaData)
        {
            int i = 0;
            List<Task> imageTasks = new List<Task>();
            foreach (string slideAddress in webMetadata.SlideAddresses)
            {
                //save each slide as slideXXX.jpg
                string localImageFileName = string.Format("slide{0}.jpg", i.ToString().PadLeft(3, '0'));
                newMetaData.SlideAddresses[i] = localImageFileName;
                i++;

                //go async!
                imageTasks.Add(assetRequester.AsyncAssetGet(slideAddress, 
                                                            repo.GetPathForAsset(AssetType.SlideImage, localImageFileName), 
                                                            null,
                                                            (s, e) => { progress.Report(localImageFileName + "download complete..."); }));                
            }

            return imageTasks;
        }

        private static IPresentationAssetsMetaData CopyBaseMetadataToLocalMetadataObject(IPresentationAssetsMetaData webMetadata)
        {
            IPresentationAssetsMetaData newMetadata = new InfoQPresentationAssetsMetaData()
            {
                FriendlyName = webMetadata.FriendlyName,
                Summary = webMetadata.Summary,
                Title = webMetadata.Title,
                //VideoLength = webMetadata.VideoLength,
                SlideStartTimes = webMetadata.SlideStartTimes,

                SlideAddresses = new string[webMetadata.SlideAddresses.Length]
            };
            return newMetadata;
        }


    }
}
