using AssetDownloader;
using AssetDownloaderTests.Mocks;
using InfoQ.Viewer.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AssetDownloaderTests
{
    [TestFixture]
    public class InfoQDownloaderTests
    {
        string testOutputPath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Files\Output\");
        string presentationUrl = @"http://www.infoq.com/presentations/ancestry-SOA-continuous-delivery";

        [Test]
        public void CanCreateInstanceOfInfoQDownloader()
        {
            string testWorkPath = Path.Combine(testOutputPath, @"\ConstructorTest\");
            Mock<IMediaRepositoryFactory> repofactory = new Mock<IMediaRepositoryFactory>();
            Mock<IAssetRequester> requester = new Mock<IAssetRequester>();
            Mock<IContentParser> parser = new Mock<IContentParser>();
            Mock<IProgress<string>> progress = new Mock<IProgress<string>>();

            IAssetDownloader downloader = new InfoQDownloader(repofactory.Object, requester.Object, parser.Object, progress.Object);

            Assert.IsNotNull(downloader, "instance not created");
            Assert.IsInstanceOf<IAssetDownloader>(downloader, "wrong interface");
            Assert.IsInstanceOf<InfoQDownloader>(downloader, "wrong class");
        }

        //missing dependency guard claus tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MissingMediaRepoInDownloaderContructorThrowsException()
        {
            Mock<IMediaRepositoryFactory> repofactory = new Mock<IMediaRepositoryFactory>();
            Mock<IAssetRequester> requester = new Mock<IAssetRequester>();
            Mock<IContentParser> parser = new Mock<IContentParser>();
            Mock<IProgress<string>> progress = new Mock<IProgress<string>>();

            IAssetDownloader downloader = new InfoQDownloader(null, requester.Object, parser.Object, progress.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MissingRequesterInDownloaderContructorThrowsException()
        {
            Mock<IMediaRepositoryFactory> repofactory = new Mock<IMediaRepositoryFactory>();
            Mock<IAssetRequester> requester = new Mock<IAssetRequester>();
            Mock<IContentParser> parser = new Mock<IContentParser>();
            Mock<IProgress<string>> progress = new Mock<IProgress<string>>();

            IAssetDownloader downloader = new InfoQDownloader(repofactory.Object, null, parser.Object, progress.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MissingParserInDownloaderContructorThrowsException()
        {
            Mock<IMediaRepositoryFactory> repofactory = new Mock<IMediaRepositoryFactory>();
            Mock<IAssetRequester> requester = new Mock<IAssetRequester>();
            Mock<IContentParser> parser = new Mock<IContentParser>();
            Mock<IProgress<string>> progress = new Mock<IProgress<string>>();

            IAssetDownloader downloader = new InfoQDownloader(repofactory.Object, requester.Object, null, progress.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MissingProgressInDownloaderContructorThrowsException()
        {
            Mock<IMediaRepositoryFactory> repofactory = new Mock<IMediaRepositoryFactory>();
            Mock<IAssetRequester> requester = new Mock<IAssetRequester>();
            Mock<IContentParser> parser = new Mock<IContentParser>();
            Mock<IProgress<string>> progress = new Mock<IProgress<string>>();

            IAssetDownloader downloader = new InfoQDownloader(repofactory.Object, requester.Object, parser.Object, null);
        }

        [Test]
        public async void CanDownloadAssetsFromTheWebToDisk()
        {
            string testWorkPath = Path.Combine(testOutputPath, @"\FakeDownloadTest\");

            //mock everything approach
            Mock<IMediaRepository> repository = new Mock<IMediaRepository>();
            repository.Setup(o => o.StoreAsset(It.IsAny<AssetType>(), It.IsAny<string>(), It.IsAny<byte[]>())).Returns(true);   //act as if it is always stored correctly
            repository.Setup(o => o.SaveMetadataFile(It.IsAny<IPresentationAssetsMetaData>())).Returns(true);
            
            Mock<IMediaRepositoryFactory> repofactory = new Mock<IMediaRepositoryFactory>();
            repofactory.Setup(o => o.Create(It.IsAny<string>())).Returns(repository.Object);
           
            Mock<IAssetRequester> requester = new Mock<IAssetRequester>();
            requester.Setup(o => o.GetHtmlContent(presentationUrl)).Returns("<html></html>");
            requester.Setup(o => o.AsyncAssetGet(It.IsAny<string>(),
                                                    It.IsAny<string>(),
                                                    It.IsAny<DownloadProgressChangedEventHandler>(),
                                                    It.IsAny<AsyncCompletedEventHandler>()))
                    .Returns(Task.FromResult("done"));
           

            Mock<IContentParser> parser = new Mock<IContentParser>();
            parser.Setup(o => o.GetAssetsMetadata(It.IsAny<string>())).Returns(DummyMetaData.GetSampleMetadata());

            Mock<IProgress<string>> progress = new Mock<IProgress<string>>();
            progress.Setup(o => o.Report(It.IsAny<string>())).Callback(() => { return; });  //do nothing

            IAssetDownloader downloader = new InfoQDownloader(repofactory.Object, requester.Object, parser.Object, progress.Object);
            Task<bool> task = downloader.DownloadPresentation(presentationUrl);
            
            bool completed = await task;

            Assert.IsTrue(completed, "download not completed");
        }

        
    
    
    
    }
}
