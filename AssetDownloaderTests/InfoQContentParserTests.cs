using AssetDownloader;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using InfoQ.Viewer.Common;

namespace InfoQAssetDownloaderTests
{
    [TestFixture]
    public class InfoQContentParserTests
    {
        string sampleValidHtml_ForArchitecting;
        string sampleValidHtml_AndroidFlexiveis;

        #region Expectations for valid file

        InfoQPresentationAssetsMetaData expectedMetadata_ForArchitecting = new InfoQPresentationAssetsMetaData()
        {
            FriendlyName = "ancestry-SOA-continuous-delivery",
            Title = "Architecting for Continuous Delivery",
            Summary = "John Esser and Russell Barnett discuss Ancestry.com’s SOA implementation capable of supporting continuous delivery, architectural standards used, and how continuous delivery works for them.",
            ThumbnailImageAddress = @"http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/mediumimage/Rusbig.jpg",

            VideoFileAddress = @"http://d1snlc0orfrhj.cloudfront.net/presentations/12-nov-architectingatancestry.mp4",
            Mp3FileAddress = @"presentations/infoq-12-nov-architectingatancestry.mp3",
            PdfFileAddress = @"presentations/QConSF2012-RussellBarnettJohnEsser-ArchitectingforContinuousDeliveryatAncestry.cm.pdf",

            SlideAddresses = new string[]{  "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl1.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl2.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl3.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl4.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl5.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl6.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl7.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl8.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl9.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl10.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl11.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl12.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl13.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl14.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl15.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl16.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl17.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl18.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl19.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl20.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl21.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl22.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl23.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl24.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl25.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl26.jpg",
                                            "http://www.infoq.com/resource/presentations/ancestry-SOA-continuous-delivery/en/slides/sl27.jpg"},
            SlideStartTimes = new int[] { 0, 30, 40, 100, 225, 280, 345, 435, 480, 505, 560, 600, 630, 685, 710, 720, 800, 1020, 1150, 1265, 1375, 1470, 1500, 1580, 1670, 1820, 1860, 2942 },
            //VideoLength = 1368060600000
        };
        int expectedMetadata_ForArchitecting_slideCount = 27;
        string expectedMetadata_ForArchitecting_slideFormat = @"http://www.infoq.com/resource/presentations/{0}/{1}/slides/sl{2}.jpg";
        string expectedMetadata_ForArchitecting_friendlyName = "ancestry-SOA-continuous-delivery";


        InfoQPresentationAssetsMetaData expectedMetadata_AndroidFlexiveis = new InfoQPresentationAssetsMetaData()
        {
            FriendlyName = "aplicacoes-android-flexiveis",
            
            Title = "Criando apps Android flex&#xED;veis e f&#xE1;ceis de manter",
            Summary = "Suas aplica&#xE7;&#xF5;es Android possuem Activities com muitas linhas de c&#xF3;digo e muitos m&#xE9;todos com switch? Comportam-se bem em Tablets ou em vers&#xF5;es anteriores do Android? Veja como deixar c&#xF3;digo e layouts mais flex&#xED;veis utilizando Fragments e Application Resources; reduza o acoplamento criando callbacks; e fa&#xE7;a bom uso das ferramentas dispon&#xED;veis na plataforma Android para facilitar a manuten&#xE7;&#xE3;o.",
            ThumbnailImageAddress = @"http://www.infoq.com/resource/presentations/aplicacoes-android-flexiveis/pt/mediumimage/Erich-Egert_270.jpg",

            VideoFileAddress = @"http://d1snlc0orfrhj.cloudfront.net/presentations-br/Mobileconf-ErichEgert-AplicacoesFlexiveis.mp4",
            Mp3FileAddress = @"presentations-br/Mobileconf-ErichEgert-AplicacoesFlexiveis.mp3",
            PdfFileAddress = @"presentations-br/Mobileconf-ErichEgert-AplicacoesFlexiveis.pdf",

            //SlideAddresses = new string[100], //too many to do this manually
            SlideStartTimes = new int[] { 0, 71, 86, 119, 129, 147, 150, 161, 176, 190, 217, 226, 240, 261, 281, 297, 317, 342, 392, 405, 418, 422, 448, 489, 491, 494, 509, 519, 537, 590, 601, 613, 619, 623, 636, 646, 665, 699, 707, 712, 745, 771, 801, 825, 845, 853, 874, 877, 896, 920, 924, 939, 960, 1051, 1062, 1069, 1081, 1089, 1110, 1127, 1151, 1181, 1203, 1233, 1271, 1306, 1350, 1364, 1373, 1405, 1412, 1460, 1478, 1495, 1507, 1520, 1574, 1596, 1638, 1664, 1675, 1729, 1773, 1804, 1810, 1845, 1857, 1876, 1915, 1936, 1963, 1966, 1973, 1980, 2002, 2051, 2080, 2156, 2174, 2183, 2855 },
            //VideoLength = 1368106200000
        };
        int expectedMetadata_AndroidFlexiveis_slideCount = 100;
        string expectedMetadata_AndroidFlexiveis_slideFormat = @"http://www.infoq.com/resource/presentations/{0}/{1}/slides/sl{2}.jpg";
        string expectedMetadata_AndroidFlexiveis_friendlyName = "aplicacoes-android-flexiveis";

        #endregion

        [TestFixtureSetUp]
        public void LoadSampleHtmlContent()
        {
            sampleValidHtml_ForArchitecting = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Files\ancestry-SOA-continuous-delivery.html"));
            sampleValidHtml_AndroidFlexiveis = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Files\aplicacoes-android-flexiveis.html"));
        }

        [Test]
        public void CanCreateInstanceOfInfoQContentParser()
        {   
            //parser sets html;
            IContentParser parser = new InfoQContentParser();

            Assert.IsNotNull(parser, "instance not created");
            Assert.IsInstanceOf<IContentParser>(parser, "wrong interface");
            Assert.IsInstanceOf<InfoQContentParser>(parser, "wrong interface");
        }

       
        [Test]
        public void CanGetValidPresentationAssetsObjectFormValidHtml()
        {
            IContentParser parser = new InfoQContentParser();
            IPresentationAssetsMetaData assets = parser.GetAssetsMetadata(sampleValidHtml_ForArchitecting);

            //no errors thrown
            AssertPresentationAssets(expectedMetadata_ForArchitecting, assets);

            Assert.Greater(assets.SlideAddresses.Length, 0, "should have images");
            for (int i = 0; i < expectedMetadata_ForArchitecting_slideCount; i++)
            {
                string expectedImage = String.Format(expectedMetadata_ForArchitecting_slideFormat, expectedMetadata_ForArchitecting_friendlyName, "en", i+1);
                Assert.AreEqual(expectedImage, assets.SlideAddresses[i], "wrong address");
            }
        }

        [Test]
        public void CanGetValidForeignPresentationAssetsObjectFormValidHtml()
        {
            IContentParser parser = new InfoQContentParser();

            IPresentationAssetsMetaData assets = parser.GetAssetsMetadata(sampleValidHtml_AndroidFlexiveis);

            //no errors thrown
            AssertPresentationAssets(expectedMetadata_AndroidFlexiveis, assets);

            Assert.Greater(assets.SlideAddresses.Length, 0, "should have images");
            for (int i = 0; i < expectedMetadata_AndroidFlexiveis_slideCount; i++)
            {
                string expectedImage = String.Format(expectedMetadata_AndroidFlexiveis_slideFormat, expectedMetadata_AndroidFlexiveis_friendlyName, "pt", i + 1);
                Assert.AreEqual(expectedImage, assets.SlideAddresses[i], "wrong address");
            }
        }
        

        [Test]
        [ExpectedException(typeof(ArgumentNullException), UserMessage="html must be suplied to parser")]
        public void GetAssetsMetadataReturnsNullIfPresentationHtmlIsNotSet()
        {
            IContentParser parser = new InfoQContentParser();
            IPresentationAssetsMetaData assets = parser.GetAssetsMetadata("");

        }





        #region Assertion Helpers

        private void AssertPresentationAssets(IPresentationAssetsMetaData expected, IPresentationAssetsMetaData parsed)
        {
            Assert.IsNotNull(parsed, "instance of assets not returned");
            Assert.IsInstanceOf<IPresentationAssetsMetaData>(parsed, "wrong interface");
            Assert.IsInstanceOf<InfoQPresentationAssetsMetaData>(parsed, "wrong interface");

            Assert.AreEqual(expected.FriendlyName, parsed.FriendlyName, "wrong name");
            Assert.AreEqual(expected.Title, parsed.Title, "wrong title");
            Assert.AreEqual(expected.Summary, parsed.Summary, "wrong summary");
            Assert.AreEqual(expected.ThumbnailImageAddress, parsed.ThumbnailImageAddress, "wrong tile image");

            Assert.AreEqual(expected.VideoFileAddress, parsed.VideoFileAddress, "wrong video file address");

            Assert.AreEqual(expected.Mp3FileAddress, parsed.Mp3FileAddress, "wrong mp3 file Address");
            Assert.AreEqual(expected.PdfFileAddress, parsed.PdfFileAddress, "wrong slide pdf file Address");

            //use manualy format and count
            //Assert.Greater(parsed.SlideAddresses.Length, 0, "should have images");
            //for (int i = 0; i < expected.SlideAddresses.Length; i++)
            //{
            //    Assert.AreEqual(expected.SlideAddresses[i], parsed.SlideAddresses[i], "wrong address");
            //}

            Assert.Greater(parsed.SlideStartTimes.Length, 0, "should have times");
            for (int i = 0; i < expected.SlideStartTimes.Length; i++)
            {
                Assert.AreEqual(expected.SlideStartTimes[i], parsed.SlideStartTimes[i], "wrong time");
            }

            //Assert.AreEqual(expected.VideoLength, parsed.VideoLength, "wrong svideo length value");
        } 

        #endregion
    }
}
