using AssetDownloader;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDownloaderTests
{
    [TestFixture]
    public class AssetRepositoryFactoryTests
    {
        [Test]
        public void CanCreateInstanceOfRepositoryFactory()
        {
            string testPath = "testPath";
            IMediaRepositoryFactory factory = new InfoQMediaRepositoryFactory(testPath);

            Assert.IsNotNull(factory, "no instance");
            Assert.IsInstanceOf<InfoQMediaRepositoryFactory>(factory, "Wrong class");
            Assert.IsInstanceOf<IMediaRepositoryFactory>(factory, "Wrong interface");
        }

        [Test]
        public void CanGetInstanceOfInfoQRepository()
        {
            string testPath = "testPath";
            
            IMediaRepositoryFactory factory = new InfoQMediaRepositoryFactory(testPath);
            IMediaRepository repo = factory.Create("testName");

            Assert.IsNotNull(repo, "no instance");
            Assert.IsInstanceOf<InfoQWindowsRepository>(repo, "Wrong class");
            Assert.IsInstanceOf<IMediaRepository>(repo, "Wrong interface");

        }

        
    }
}
