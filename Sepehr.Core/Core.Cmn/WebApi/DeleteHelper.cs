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
    public class DeleteHelper
    {
        public static string Delete(
            string url,
            List<KeyValuePair<string, string>> headers,
            CookieCollection cookies
            )
        {
            HttpClientHandler handler = WebApiHelper.GetHttpClientHandler(cookies, url);

            using (HttpClient httpClient = new HttpClient(handler))
            {
                WebApiHelper.SetAcceptHeader(httpClient, new MediaTypeWithQualityHeaderValue("application/json"));
                WebApiHelper.SetHeaders(httpClient, headers, true);

                Task<HttpResponseMessage> httpResponseMessageTask = httpClient.DeleteAsync(url);
                HttpResponseMessage httpResponseMessage = httpResponseMessageTask.Result;
                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return result;
                }
                else
                {
                    throw WebApiHelper.GetException(result, httpResponseMessage);
                }
            }
        }

        public static T WebApiDelete<T>(string url, List<KeyValuePair<string, string>> headers)
        {
            var result = Delete(
                url, 
                headers,
                null//cookies
                );
            return result.DeSerializeJSONToObject<T>();
        }

        public static T WebApiDelete<T>(string url)
        {
            var result = Delete(
                url, 
                null,//headers
                null//cookies
                );

            return result.DeSerializeJSONToObject<T>();
        }

        public static string WebApiDelete(string url)
        {
            return Delete(
                url, 
                null,//headers
                null//cookies
                );
        }
    }
}