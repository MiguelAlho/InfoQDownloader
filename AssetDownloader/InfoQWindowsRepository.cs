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
    
    public class InfoQWindowsRepository : IMediaRepository
    {
        string basePath;
        string presentationName;
        
        public InfoQWindowsRepository(string mediaFolderPath, string presentationName)
        {
            //TODO: tests for guardclauses
            if(string.IsNullOrWhiteSpace(mediaFolderPath))
                throw new ArgumentNullException("media base path must be specified");
            if (string.IsNullOrWhiteSpace(presentationName))
                throw new ArgumentNullException("presentation name must be specified");

            basePath = mediaFolderPath;
            this.presentationName = presentationName;
        }


        public string CreateRepositoryForPresentation()
        {
            string path = Path.Combine(basePath, presentationName + "\\");
            if(Directory.Exists(path))
                throw new IOException("Presentation folder already exists. Remove before recreating.");

            Directory.CreateDirectory(Path.Combine(path, @"Slides\"));
            Directory.CreateDirectory(Path.Combine(path, @"Thumbs\"));
            Directory.CreateDirectory(Path.Combine(path, @"Video\"));
            Directory.CreateDirectory(Path.Combine(path, @"Audio\"));
            Directory.CreateDirectory(Path.Combine(path, @"PDF\"));

            return path;
        }

        [Obsolete("Tie downloads directly to download stream and asyn methodology")]
        public bool StoreAsset(AssetType assetType, string fileName, byte[] assetData)
        {
            string assetpath = Path.Combine(basePath, presentationName, FolderFromAssetType(assetType), fileName);
            File.WriteAllBytes(assetpath, assetData);
            return true;
        }        

        public bool SaveMetadataFile(IPresentationAssetsMetaData metadata)
        {
            string metadatapath = Path.Combine(basePath, presentationName, "metadata.xml");
            using(FileStream stream = new FileStream(metadatapath, FileMode.Create))
            {
                XmlSerializer x = new XmlSerializer(metadata.GetType());
                x.Serialize(stream, metadata);
                return true;
            }
        }


        public string GetPathForAsset(AssetType assetType, string fileName)
        {
            return Path.Combine(basePath, presentationName, FolderFromAssetType(assetType), fileName);
        }

        private string FolderFromAssetType(AssetType assetType)
        {
            switch (assetType)
            {
                case AssetType.Video:
                    return @"Video\";
                case AssetType.ThumbnailImage:
                    return @"Thumbs\";
                case AssetType.SlideImage:
                    return @"Slides\";
                case AssetType.PDF:
                    return @"PDF\";
                case AssetType.MP3:
                    return @"Audio\";
                case AssetType.MetaData:
                    return @"\";
            }
            return @"\";
        }
    }
}
