using AssetDownloader;
using AssetDownloaderTests.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AssetDownloaderTests
{
    [TestFixture]
    public class InfoQAssetRequesterTests
    {
        string validTestAddress = @"http://www.infoq.com/presentations/ancestry-SOA-continuous-delivery";
        //string validBrTestAddress = @"http://www.infoq.com/br/presentations/aplicacoes-android-flexiveis";
        //string validCnTestAddress = @"http://www.infoq.com/cn/presentations/aplicacoes-android-flexiveis";
        //string validJpTestAddress = @"http://www.infoq.com/jp/presentations/aplicacoes-android-flexiveis";
        //string testProtoTestAddress = @"test://www.infoq.com/presentations/ancestry-SOA-continuous-delivery";        
        
        string invalidTestAddress = @"https://www.infoq.com/presentations/ancestry-SOA-continuous-delivery";    //address should be http:

        //string validImageAsset = @"http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl1.jpg";
            
        [Test]
        public void CanCreateInstanceOfAssetRequester()
        {
            IAssetRequester requester = new InfoQAssetRequester();

            Assert.IsNotNull(requester, "no instance");
            Assert.IsInstanceOf<InfoQAssetRequester>(requester, "Wrong class");
            Assert.IsInstanceOf<IAssetRequester>(requester, "Wrong interface");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), UserMessage = "an exception should be thrown if the address is missing in the GetHtmlContent Method")]
        public void MissingAddressThrowsException()
        {
            IAssetRequester requester = new InfoQAssetRequester();
            string response = requester.GetHtmlContent(null);            
            
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), UserMessage = "an exception should be thrown if the address is an empty string in the GetHtmlContent Method")]
        public void EmptyAddressThrowsException()
        {
            IAssetRequester requester = new InfoQAssetRequester();
            string response = requester.GetHtmlContent("");  
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), UserMessage = "Should return exception for invalid address")]
        public void IncorectlyFormedAddressThrowsException()
        {
            IAssetRequester requester = new InfoQAssetRequester();
            string response = requester.GetHtmlContent(invalidTestAddress);  
        }

        
        [Test]
        [Ignore("Uses real HttpRequest. Use for debugging only.")]
        public void CanGetHtmlFromWebForAGivenInfoQAddress()
        {
            
            string address = validTestAddress;
            IAssetRequester requester = new InfoQAssetRequester();

            string response = requester.GetHtmlContent(validTestAddress);            
            Assert.IsNotNullOrEmpty(response, "unexpected empty or null response");

        }


        /*

        [Test]
        [Ignore("Uses real HttpRequest. Use for debugging only.")]
        public void CanGetAssetFromWebForAGivenInfoQAddress()
        {
            string address = validTestAddress;
            IAssetRequester requester = new InfoQAssetRequester();

            byte[] response = requester.GetMediaAsset(validImageAsset);
            Assert.IsNotNull(response, "unexpected empty or null response");
            Assert.Greater(response.Length, 0, "unexpected empty or null response");

        }
        */
    }
}
