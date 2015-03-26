using AssetDownloader;
using AssetDownloaderTests.Mocks;
using InfoQ.Viewer.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDownloaderTests
{
    [TestFixture]
    public class InfoQWindowsRepositoryTests
    {
        string testOutputPath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Files\Output\RepoTests\");

        [Test]
        public void CanCreateInstanceOfInfoQWindowsRepository()
        {
            //string testPath = Path.Combine(testOutputPath);
            string presentation = "testPresentation";
            IMediaRepository repo = new InfoQWindowsRepository(testOutputPath, presentation);

            Assert.IsNotNull(repo, "instance not created");
            Assert.IsInstanceOf<IMediaRepository>(repo, "wrong interface");
            Assert.IsInstanceOf<InfoQWindowsRepository>(repo, "wrong concrete class");
        }

        [Test]
        public void CanCreateMediaDirectory()
        {
            string presentationname = "ancestry-SOA-continuous-delivery";

            string expectedDirName = @"ancestry-SOA-continuous-delivery";
            string expectedBasePath = Path.Combine(testOutputPath, expectedDirName + "\\");
            string expectedSlideFolder = Path.Combine(expectedBasePath, @"Slides\");
            string expectedThumbFolder = Path.Combine(expectedBasePath, @"Thumbs\");
            string expectedVideoFolder = Path.Combine(expectedBasePath, @"Video\");
            string expectedAudioFolder = Path.Combine(expectedBasePath, @"Audio\");
            string expectedPDFFolder = Path.Combine(expectedBasePath, @"PDF\");

            //pretest cleanup
            if(Directory.Exists(expectedBasePath))
                Directory.Delete(expectedBasePath, true);

            //mck dependencies
            IMediaRepository repo = new InfoQWindowsRepository(testOutputPath, presentationname);
            string createdpath = repo.CreateRepositoryForPresentation();

            Assert.IsTrue(Directory.Exists(expectedBasePath), "base dir is missing");
            Assert.AreEqual(expectedBasePath, createdpath, "wrong path returned");
            Assert.IsTrue(Directory.Exists(expectedSlideFolder), "slide dir is missing");
            Assert.IsTrue(Directory.Exists(expectedThumbFolder), "thumb dir is missing");
            Assert.IsTrue(Directory.Exists(expectedVideoFolder), "video dir is missing");
            Assert.IsTrue(Directory.Exists(expectedAudioFolder), "audio dir is missing");
            Assert.IsTrue(Directory.Exists(expectedPDFFolder), "pdf dir is missing");

            //postTest cleanup
            if (Directory.Exists(expectedBasePath))
                Directory.Delete(expectedBasePath, true);
        }

        [Test]
        public void CanSerializePresentationMetadata()
        {
            string presentationname = "ancestry-SOA-continuous-delivery";
            string expectedDirName = @"ancestry-SOA-continuous-delivery";
            string expectedBasePath = Path.Combine(testOutputPath, expectedDirName + "\\");
            string expectedMetaFilePath = Path.Combine(expectedBasePath + "\\", "metadata.xml");
           
            //pretest cleanup
            if (Directory.Exists(expectedBasePath))
                Directory.Delete(expectedBasePath, true);

            //mck dependencies
            IMediaRepository repo = new InfoQWindowsRepository(testOutputPath, presentationname);
            string createdpath = repo.CreateRepositoryForPresentation();
            IPresentationAssetsMetaData metadata = DummyMetaData.GetSampleMetadata();

            //act
            repo.SaveMetadataFile(metadata);

            //assert
            Assert.IsTrue(File.Exists(expectedMetaFilePath), "file should exist");

            //postTest cleanup
            if (Directory.Exists(expectedBasePath))
                Directory.Delete(expectedBasePath, true);
        }

        [Test]
        public void CanSaveMetadata()
        {
            string presentationname = "ancestry-SOA-continuous-delivery";
            string expectedDirName = @"ancestry-SOA-continuous-delivery";
            string expectedBasePath = Path.Combine(testOutputPath, expectedDirName + "\\");
            string expectedImagePath = Path.Combine(expectedBasePath + "\\", "Slides\\sl1.jpg");
            byte[] mockImage = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Files\sl1.jpg"));

            //pretest cleanup
            if (Directory.Exists(expectedBasePath))
                Directory.Delete(expectedBasePath, true);

            //mck dependencies
            IMediaRepository repo = new InfoQWindowsRepository(testOutputPath, presentationname);
            string createdpath = repo.CreateRepositoryForPresentation();
            IPresentationAssetsMetaData metadata = DummyMetaData.GetSampleMetadata();

            //act
            repo.StoreAsset(AssetType.SlideImage, "sl1.jpg", mockImage  );

            //assert
            Assert.IsTrue(File.Exists(expectedImagePath), "file should exist");

            //postTest cleanup
            if (Directory.Exists(expectedBasePath))
                Directory.Delete(expectedBasePath, true);
        }
    }
}
