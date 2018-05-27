using Core.Cmn.Extensions;
using Core.Cmn.WebApi.Base;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.WebApi
{
    public class PutHelper
    {
        private static WebApiHelperResponse Put(
            string url,
            List<KeyValuePair<string, string>> headers,
            CookieCollection cookies,
            string content
            )
        {
            HttpClientHandler handler = WebApiHelper.GetHttpClientHandler(cookies, url);

            using (HttpClient httpClient = new HttpClient(handler))
            {
                WebApiHelper.SetAcceptHeader(httpClient, new MediaTypeWithQualityHeaderValue("application/json"));
                WebApiHelper.SetHeaders(httpClient, headers, true);

                Task<HttpResponseMessage> httpResponseMessageTask = httpClient.PutAsync(
                    url,
                    new StringContent(
                        content,
                        Encoding.UTF8,
                        "application/json"
                        ));

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

        public static T WebApiPut<T>(string url, List<KeyValuePair<string, string>> headers, string content)
        {
            var result = Put(
                url,
                headers,
                null,//cookies
                content
                );

            return result.Content.DeSerializeJSONToObject<T>();
        }

        public static T WebApiPut<T>(string url, string content)
        {
            var result = Put(
                url,
                null,//headers
                null,//cookies
                content
                );

            return result.Content.DeSerializeJSONToObject<T>();
        }

        public static string WebApiPut(string url, string content)
        {
            return Put(
                url,
                null,//headers
                null,//cookies
                content
                ).Content;
        }
    }
}