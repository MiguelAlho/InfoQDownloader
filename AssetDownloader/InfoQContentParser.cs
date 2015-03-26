using InfoQ.Viewer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssetDownloader
{
    public class InfoQContentParser : IContentParser
    {
        string webRoot = "http://www.infoq.com";

        public InfoQContentParser()
        {
            
        }

        public IPresentationAssetsMetaData GetAssetsMetadata(string htmlToParse)
        {
            if (string.IsNullOrWhiteSpace(htmlToParse))
                throw new ArgumentNullException("Html required to use parser");

            InfoQPresentationAssetsMetaData metadata = new InfoQPresentationAssetsMetaData();

            metadata.Title = GetTitleFromHtml(htmlToParse);
            metadata.Summary = GetSummaryFromHtml(htmlToParse);
            metadata.ThumbnailImageAddress = GetThumbnailImageAddressFromHtml(htmlToParse);
            metadata.VideoFileAddress = GetVideoFileAddressFromHtml(htmlToParse);
            metadata.Mp3FileAddress = GetMp3FileAddressFromHtml(htmlToParse);
            metadata.PdfFileAddress = GetPdfFileAddressFromHtml(htmlToParse);
            metadata.SlideAddresses = GetSlideAddressesFromHtml(htmlToParse);
            metadata.SlideStartTimes = GetTimesFromHtml(htmlToParse);

            //test this, but it occures when thumb info is not found - use Events-Are-NOT-Just-for-Notifications presentation
            if (metadata.ThumbnailImageAddress != "http://www.infoq.com")
            {
                metadata.FriendlyName = GetFriendlyNameFromThumbAddress(metadata.ThumbnailImageAddress);
            }
            else
            {
                metadata.FriendlyName = GetFriendlyNameFromTitle(metadata.Title);
            }

            return metadata;
        }

        private int[] GetTimesFromHtml(string htmlToParse)
        {
            string regexMatchPdf = @"TIMES = new Array\(([\d*,\s]*)\)";
            int[] timeValues = GetStringFromHtml(htmlToParse, regexMatchPdf)
                                    .Split(new char[]{','})
                                    .ToList()
                                    .Select(o => int.Parse(o))
                                    .ToArray();
            return timeValues;
        }

        private string[] GetSlideAddressesFromHtml(string htmlToParse)
        {
            List<string> addresses = new List<string>();
            try
            {
                Regex regexObj = new Regex(@"/resource/presentations/[\w\-]*/[\w]*/slides/[A-Za-z\-0-9]*.jpg");
                Match matchResults = regexObj.Match(htmlToParse);
                while (matchResults.Success)
                {
                    // matched text: matchResults.Value
                    // match start: matchResults.Index
                    // match length: matchResults.Length
                    addresses.Add(webRoot + matchResults.Value);
                    matchResults = matchResults.NextMatch();
                }
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException("Can't extract presentation slides from the given html");
            }

            return addresses.ToArray();
        }

        private string GetPdfFileAddressFromHtml(string htmlToParse)
        {
            string regexMatchPdf = @"value=""(presentations[\-]*[br]*/[\w\-.]*.pdf)""/>";
            return GetStringFromHtml(htmlToParse, regexMatchPdf);
        }

        private string GetMp3FileAddressFromHtml(string htmlToParse)
        {
            string regexMatchMp3 = @"value=""(presentations[\-]*[br]*/[\w\-.]*.mp3)""/>";
            return GetStringFromHtml(htmlToParse,regexMatchMp3);
        }

        private string GetVideoFileAddressFromHtml(string htmlToParse)
        {
            string regexMatchMp4 = @"<source\ssrc=""(http://[\w\d]*.cloudfront.net/presentations[\-]*[br]*/[\w\-_.]*.mp4)""\s*/>";
            return GetStringFromHtml(htmlToParse, regexMatchMp4);
        }

        private string GetThumbnailImageAddressFromHtml(string htmlToParse)
        {
            string regexMatchThumbImage = @"<link\srel=""image_src""\shref=""(/resource/presentations/[\w\-]*/[\w]*/mediumimage/[\w\-_]*.jpg)""/>";
            return webRoot + GetStringFromHtml(htmlToParse, regexMatchThumbImage);
        }

        private string GetFriendlyNameFromThumbAddress(string thumbImageAddress)
        {   
            //@"http://www.infoq.com/resource/presentations/aplicacoes-android-flexiveis/pt/mediumimage/Erich-Egert_270.jpg",

            string regexMatchThumbImage = webRoot + @"/resource/presentations/([\w\-]*)/[\w]*/mediumimage/[\w\-_]*.jpg";
            return GetStringFromHtml(thumbImageAddress, regexMatchThumbImage);
        }

        private string GetFriendlyNameFromTitle(string title)
        {
           return title.Replace(" ","-");
        }


        private string GetSummaryFromHtml(string htmlToParse)
        {
            string regexMatchSummary = @"<meta\s*name=""description""\s*content=""([\w\s.'’,&#;?\-]*)""/>";
            return GetStringFromHtml(htmlToParse, regexMatchSummary);
        }

        private string GetTitleFromHtml(string htmlToParse)
        {
            string regexMatchTitle = @"<h1[A-Za-z0-9\s=""]*>[\s]*<div>([\w\s&#;]*)</div>";
            return GetStringFromHtml(htmlToParse,regexMatchTitle);
        }

        private string GetStringFromHtml(string htmlToParse, string regexMatchString)
        {
            string resultString;

            try
            {
                //group 0 is the full match; group1 has the summary string
                resultString = Regex.Match(htmlToParse, regexMatchString).Groups[1].Value;
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException("Can't extract presentation summary from the given html");
            }

            return resultString;
        }
    }
}
