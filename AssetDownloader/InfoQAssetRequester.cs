using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssetDownloader
{
    public class InfoQAssetRequester : IAssetRequester
    {
        string infoQRegexMatcher = @"\A(?:http://www\.infoq\.com(/br|/cn|/jp|)/presentations/[-A-Za-z0-9_.]*)\Z";

        public InfoQAssetRequester()
        {            
        }

        public string GetHtmlContent(string presentationAddress)
        {
            if (String.IsNullOrWhiteSpace(presentationAddress))
                throw new ArgumentNullException("presentationAddress", "presentationAddress can not be null nor empty.");

            //test address format
            if (!Regex.IsMatch(presentationAddress, infoQRegexMatcher))
                throw new ArgumentException(presentationAddress + " is not a valid infoQ presentation address.");

            //Request URL:http://www.infoq.com/presentations/ancestry-SOA-continuous-delivery
            //Request Method:GET
            //Status Code:200 OK
            //Request Headersview source
            //Accept:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8
            //Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.3
            //Accept-Encoding:gzip,deflate,sdch
            //Accept-Language:pt-BR,pt;q=0.8,en-US;q=0.6,en;q=0.4
            //Cache-Control:no-cache
            //Connection:keep-alive
            //Cookie:__gads=ID=49da7842f5542148:T=1354713844:S=ALNI_MbdWDF7o3c1k2TENBi4epCF5nk8yA; UserCookie=8cRKNAa9qbslBUvaQVqGFVJMxwEtnzMJ; RegisteredUserCookie=fAe7bytxQtBR7KUV4F7P8gQcnTCDMxYF; _sm_au_c=iHMNNRMdQ4sjFVQP0a; nbi=11; JSESSIONID=1EBA7FDEDE2D34E7794868333F312379; __utma=213602508.1023675594.1354713838.1368214628.1368214629.350; __utmb=213602508.79.2.1368215382558; __utmc=213602508; __utmz=213602508.1368214629.350.140.utmcsr=feedly|utmccn=(not%20set)|utmcmd=(not%20set); __atuvc=59%7C15%2C27%7C16%2C42%7C17%2C48%7C18%2C127%7C19
            //Host:www.infoq.com
            //Pragma:no-cache
            //User-Agent:Mozilla/5.0 (iPad; CPU OS 4_3_2 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8H7 Safari/6533.18.5
            //Response Headersview source
            //Connection:Keep-Alive
            //Content-Type:text/html;charset=utf-8
            //Date:Fri, 10 May 2013 19:50:22 GMT
            //Keep-Alive:timeout=5, max=100
            //Server:Apache/2.2.15 (Red Hat)
            //Transfer-Encoding:chunked

            WebRequest request = WebRequest.Create(presentationAddress);
            request.Method = "GET";
            request.ContentType = "text/html";
            ((HttpWebRequest)request).UserAgent = @"Mozilla/5.0 (iPad; CPU OS 4_3_2 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8H7 Safari/6533.18.5";

            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string responseString = reader.ReadToEnd();
                return responseString;
            }
        }

        public Task AsyncAssetGet(string assetAddress, string fileSavePath, DownloadProgressChangedEventHandler progressHandler, AsyncCompletedEventHandler completedHandler)
        {
            var client = new WebClient();
            Uri address = new Uri(assetAddress);
           
            if(progressHandler != null)
                client.DownloadProgressChanged += progressHandler;
            if(completedHandler != null)
                client.DownloadFileCompleted += completedHandler;
            
            return client.DownloadFileTaskAsync(address, fileSavePath);
        }
    }
}
