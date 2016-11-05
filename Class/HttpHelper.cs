using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace AcFunVideo.Class
{


    internal static class HttpHelper
    {

        internal static void CreateHttpClient(ref HttpClient httpClient)
        {
            if (null!=httpClient)
            {
                httpClient.Dispose();
            }

            IHttpFilter filter = new HttpBaseProtocolFilter();
            filter = new PlugInFilter(filter);

            httpClient = new HttpClient(filter);
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new Windows.Web.Http.Headers.HttpProductInfoHeaderValue("AcFun/4.1.4 (iPhone; iOS 9.2.1; Scale/2.00)"));
        }
    }

    public class PlugInFilter : IHttpFilter
    {
        private IHttpFilter innerFilter;

        public PlugInFilter(IHttpFilter innerFilter)
        {
            if (innerFilter == null)
            {
                throw new ArgumentException("innerFilter cannot be null.");
            }
            this.innerFilter = innerFilter;
        }

        public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(HttpRequestMessage request)
        {
            return AsyncInfo.Run<HttpResponseMessage, HttpProgress>(async (cancellationToken, progress) =>
            {
                request.Headers.Add("Custom-Header", "CustomRequestValue");
                HttpResponseMessage response = await innerFilter.SendRequestAsync(request).AsTask(cancellationToken, progress);

                cancellationToken.ThrowIfCancellationRequested();

                response.Headers.Add("Custom-Header", "CustomResponseValue");
                return response;
            });
        }

        public void Dispose()
        {
            innerFilter.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
