using AssetDownloader;
using InfoQ.Viewer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDownloaderTests.Mocks
{
    public class DummyMetaData
    {
        public static IPresentationAssetsMetaData GetSampleMetadata()
        {
            return new InfoQPresentationAssetsMetaData()
            {
                FriendlyName = "sample_presentation",
                Title = "Sample Presentation",
                Summary = "Sample summary.",
                ThumbnailImageAddress = @"http://www.infoq.com/resource/presentations/sample_presentation/en/mediumimage/sample.jpg",
                VideoFileAddress = @"http://d1snlc0orfrhj.cloudfront.net/presentations/sample_presentation.mp4",
                Mp3FileAddress = @"presentations/isample_presentation.mp3",
                PdfFileAddress = @"presentations/sample_presentation.pdf",

                SlideAddresses = new string[]{  "http://www.infoq.com/resource/presentations/sample_presentation/en/slides/sl1.jpg",
                                                "http://www.infoq.com/resource/presentations/sample_presentation/en/slides/sl2.jpg",
                                                "http://www.infoq.com/resource/presentations/sample_presentation/en/slides/sl3.jpg"},
                SlideStartTimes = new int[] { 0, 30, 40 },
                VideoLength = 40000
            } as IPresentationAssetsMetaData;

        }
    }
}
