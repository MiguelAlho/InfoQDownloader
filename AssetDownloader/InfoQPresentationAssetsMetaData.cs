using InfoQ.Viewer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetDownloader
{
    [Serializable]
    public class InfoQPresentationAssetsMetaData : IPresentationAssetsMetaData  
    {
        List<string> slides = new List<string>();
        List<int> times = new List<int>();

        public string FriendlyName { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string ThumbnailImageAddress { get; set; }
        public string VideoFileAddress { get; set; }
        public string Mp3FileAddress { get; set; }
        public string PdfFileAddress { get; set; }
        public string[] SlideAddresses
        {
            get { return slides.ToArray(); }
            set
            {
                slides = new List<string>();
                slides.AddRange(value);
            }
        }
        public int[] SlideStartTimes {
            get { return times.ToArray(); }
            set
            {
                times = new List<int>(); 
                times.AddRange(value);
            }
        }
        public long VideoLength { get; set; }
    }
}
