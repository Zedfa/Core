using Core.Cmn.Extensions;
using Core.Cmn.WebApi.Base;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Core.Cmn.WebApi
{
    public class GetHelper
    {
        private static WebApiHelperResponse Get(
            string url,
            List<KeyValuePair<string, string>> headers,
            CookieCollection cookies,
            TimeSpan? timeout,
            MediaTypeWithQualityHeaderValue accept
            )
        {
            HttpClientHandler handler = WebApiHelper.GetHttpClientHandler(cookies, url);

            using (HttpClient httpClient = new HttpClient(handler))
            {
                WebApiHelper.SetHttpClientTimeOut(httpClient, timeout);
                WebApiHelper.SetAcceptHeader(httpClient, accept);

                WebApiHelper.SetHeaders(httpClient, headers, true);

                Task<HttpResponseMessage> httpResponseMessageTask = httpClient.GetAsync(url);

                HttpResponseMessage httpResponseMessage = httpResponseMessageTask.Result;
                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new WebApiHelperResponse
                    {
                        Content = result,
                        Headers = httpResponseMessage.Headers,
                        Cookies = handler.CookieContainer.GetCookies(new Uri(url))
                    };
                }
                else
                {
                    throw WebApiHelper.GetException(result, httpResponseMessage);
                }
            }
        }

        public static T WebApiGet<T>(string url, List<KeyValuePair<string, string>> headers)
        {
            var content = Get(
                url,
                headers,
                null,//cookies
                null,//timeout
                null//accept
                );

            return content.Content.DeSerializeJSONToObject<T>();
        }

        public static T WebApiGet<T>(string url)
        {
            var content = Get(
                url,
                null,//headers
                null,//cookies
                null,//timeout
                null//accept
                );

            return content.Content.DeSerializeJSONToObject<T>();
        }

        public static T WebApiGet<T>(string url, TimeSpan timeout)
        {
            var content = Get(
                url,
                null,//headers
                null,//cookies
                null,//timeout
                null//accept
                );

            return content.Content.DeSerializeJSONToObject<T>();
        }

        public static string WebApiGet(string url, TimeSpan timeout)
        {
            return Get(
                url,
                null,//headers
                null,//cookies
                timeout,
                null//accept
                ).Content;
        }

        public static string WebApiGet(string url)
        {
            return Get(
                url,
                null,//headers
                null,//cookies
                null,//timeout
                null//accept
                ).Content;
        }
    }
}