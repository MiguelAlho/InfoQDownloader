using System;
namespace InfoQ.Viewer.Common
{
    public interface IPresentationAssetsMetaData
    {
        string FriendlyName { get; set; }
        string Mp3FileAddress { get; set; }
        string PdfFileAddress { get; set; }
        string[] SlideAddresses { get; set; }
        int[] SlideStartTimes { get; set; }
        string Summary { get; set; }
        string ThumbnailImageAddress { get; set; }
        string Title { get; set; }
        string VideoFileAddress { get; set; }
        //long VideoLength { get; set; }  //main value from page
    }

    public interface IPresentation : IPresentationAssetsMetaData
    {
        int ViewedTime { get; set; }    //these should be in seconds
        int TotalTime { get; set; }     //these should be in seconds, from VideoLength
        int Percentage { get; }

        StorageType StorageLocation { get; set; }


    }
}
