using InfoQ.Viewer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AssetDownloader
{
    public interface IAssetRequester
    {
        string GetHtmlContent(string presentationAddress);
        //byte[] GetMediaAsset(string assetAddress);
        Task AsyncAssetGet(string assetAddress, string fileSavePath, DownloadProgressChangedEventHandler progressHandler, AsyncCompletedEventHandler completedHandler);
    }

    
    public interface IContentParser
    {
        IPresentationAssetsMetaData GetAssetsMetadata(string htmlToParse);
    }

    public interface IAssetDownloader
    {
        Task<bool> DownloadPresentation(string presentationAddress); //TODO inject these, string saveLocation (from config), IAssetRequester requester, IContentParser parser );
    }

    public interface IMediaRepositoryFactory
    {
        IMediaRepository Create(string presentationName);
    }

    public interface IMediaRepository
    {
        //returns dir path
        string CreateRepositoryForPresentation(); //string presentationName);

        bool StoreAsset(AssetType assetType, string fileName, byte[] assetData);

        bool SaveMetadataFile(IPresentationAssetsMetaData metadata);

        string GetPathForAsset(AssetType assetType, string fileName);
    }
    

}
