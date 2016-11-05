using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace AcFunVideo.Class
{
    public class HttpEngine
    {
     
            HttpClient httpclient;
        private CancellationTokenSource cts;
        public async Task<IInputStream> Get(string url)
        {

         var request=   this.CreateMessage(HttpMethod.Get, new Uri(url));
            HttpHelper.CreateHttpClient(ref httpclient);
            cts = new CancellationTokenSource();
            request.Headers["User-Agent"] = "AcFun/4.1.4(iPhone;iOS 9.2.1;Scale/2.00)";
            request.Headers["deviceType"] = "0";
            request.Headers["appVersion"] = "4.1.4";
            request.Headers["market"] = "appstore";
            request.Headers["productId"] = "2000";
            request.Headers["resolution"] = "750x1334";
            try
            {
                HttpResponseMessage response = await httpclient.SendRequestAsync(
             request,
             HttpCompletionOption.ResponseContentRead).AsTask(cts.Token);
                var responseStream = await response.Content.ReadAsInputStreamAsync();
                return responseStream;
            }
            catch ( Exception ex )
            {
                ACDEBUG.Print(ex.Message);
            }
            return null;
        }

        public async void Get(string uri,List<KeyValuePair<string,string>> headers)
        {
           
        }

        private HttpRequestMessage CreateMessage(HttpMethod method,Uri uri)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, uri);
            return request;
        }

        private HttpRequestMessage CreateMessage(HttpMethod method,Uri uri,List<KeyValuePair<string,string>> headers)
        {
            var request = this.CreateMessage(method, uri);
            foreach (var item in headers)
            {
                request.Headers.Add(item);
            }
            return request;
        }
    }
}
