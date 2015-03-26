using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace InfoQ.Viewer.Common
{

    public class Presentation : IPresentation
    {
        public string FriendlyName { get; set; }    //works like an ID

        public string Title { get; set; }
        public string Summary { get; set; }
        public string ThumbnailImageAddress { get; set; }
        
        public string Mp3FileAddress { get; set; }
        public string PdfFileAddress { get; set; }
        public string[] SlideAddresses { get; set; }
        public int[] SlideStartTimes { get; set; }
        
        public string VideoFileAddress { get; set; }

        //LocalAppData
        public StorageType StorageLocation { get; set; }
        public int ViewedTime { get; set; } //seconds
        public int TotalTime {get; set;}

        /// <summary>
        /// Returns the precentage of video returned. Min is 0 (when times unkown) and maxes out at 100
        /// </summary>
        public int Percentage
        {
            get
            {
                if (TotalTime > 0)
                {
                   int percentage = Convert.ToInt32(Math.Floor((double)(ViewedTime * 100) / (double)TotalTime));
                   if (percentage > 100)
                       return 100;
                   return percentage;
                }
                return 0;
            }
        }
    }
}
