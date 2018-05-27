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
    public class PostHelper
    {
        private static WebApiHelperResponse Post(
            string url,
            List<KeyValuePair<string, string>> headers,
            CookieCollection cookies,
            HttpContent content,
            TimeSpan? timeout,
            MediaTypeWithQualityHeaderValue accept,
            bool validateHeaders = true
            )
        {
            HttpClientHandler handler = WebApiHelper.GetHttpClientHandler(cookies, url);

            using (HttpClient httpClient = new HttpClient(handler))
            {
                WebApiHelper.SetHttpClientTimeOut(httpClient, timeout);
                WebApiHelper.SetAcceptHeader(httpClient, accept);
                WebApiHelper.SetHeaders(httpClient, headers, validateHeaders);

                Task<HttpResponseMessage> httpResponseMessageTask = httpClient.PostAsync(url, content);
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

        public static string WebApiPost(
            string url, 
            List<KeyValuePair<string, string>> headers, 
            HttpContent content, 
            TimeSpan? timeout
            )
        {
            return Post(
                url,
                headers,
                null,//cookies
                content,
                timeout,
                null,//accept
                true//validateHeaders
                ).Content;
        }

        public static WebApiHelperResponse WebApiPost(
            string url,
            List<KeyValuePair<string, string>> headers,
            string content,
            TimeSpan? timeout
            )
        {
            return Post(
                url,
                headers,
                null,//cookies
                new StringContent(content, Encoding.UTF8, "application/json"),//content
                timeout,
                null,//accept
                true//validateHeaders
                );
        }

        public static WebApiHelperResponse WebApiPost(
            string url,
            List<KeyValuePair<string, string>> headers,
            CookieCollection cookies,
            HttpContent content,            
            MediaTypeWithQualityHeaderValue accept,
            bool validateHeaders = true
            )
        {
            return Post(
                url,
                headers,
                cookies,
                content,
                null,//timeout
                accept,
                validateHeaders
                );
        }

        public static WebApiHelperResponse WebApiPost(
            string url,
            List<KeyValuePair<string, string>> headers,
            HttpContent content,
            TimeSpan? timeout,
            MediaTypeWithQualityHeaderValue accept,
            bool validateHeaders = true
            )
        {
            return Post(
                url,
                headers,
                null,//cookies
                content,
                timeout,
                accept,
                validateHeaders
                );
        }

        public static T WebApiPost<T>(string url, List<KeyValuePair<string, string>> headers, HttpContent content)
        {
            var result = Post(
                url,
                headers,
                null,//cookies
                content,
                null,//timeout
                null,//accept
                true//validateHeaders
                );

            return result.Content.DeSerializeJSONToObject<T>();
        }

        public static T WebApiPost<T>(string url, List<KeyValuePair<string, string>> headers, string content)
        {
            var result = WebApiPost(url, headers, content, null);
            return result.Content.DeSerializeJSONToObject<T>();
        }

        public static T WebApiPost<T>(string url, HttpContent content)
        {
            var result = Post(
                url,
                null,//headers
                null,//cookies
                content,
                null,//timeout
                null,//accept
                true//validateHeaders
                );

            return result.Content.DeSerializeJSONToObject<T>();
        }

        public static T WebApiPost<T>(string url, string content)
        {
            var result = Post(
                url,
                null,//headers
                null,//cookies
                new StringContent(content, Encoding.UTF8, "application/json"),//content
                null,//timeout
                null,//accept
                true//validateHeaders
                );

            return result.Content.DeSerializeJSONToObject<T>();
        }

        public static string WebApiPost(string url, HttpContent content)
        {
            return Post(
                url,
                null,//headers
                null,//cookies
                content,//content
                null,//timeout
                null,//accept
                true//validateHeaders
                ).Content;
        }

        public static WebApiHelperResponse WebApiPost(string url, string content)
        {
            return Post(
                url,
                null,//headers
                null,//cookies
                new StringContent(content, Encoding.UTF8, "application/json"),//content
                null,//timeout
                null,//accept
                true//validateHeaders
                );
        }

        public static WebApiHelperResponse WebApiPost(string url, string content, CookieCollection cookies)
        {
            return Post(
                url,
                null,//headers
                cookies,//cookies
                new StringContent(content, Encoding.UTF8, "application/json"),//content
                null,//timeout
                null,//accept
                true//validateHeaders
                );
        }
    }
}