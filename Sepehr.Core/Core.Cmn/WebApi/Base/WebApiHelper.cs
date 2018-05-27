using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Core.Cmn.WebApi.Base
{
    public class WebApiHelper
    {
        public static Exception GetException(
            string result,
            HttpResponseMessage httpResponseMessage
            )
        {
            if (!string.IsNullOrWhiteSpace(result))
            {
                return new Exception(result);
            }
            else
            {
                /// chon ye vaght haee (mesle Api caspian) error tavasote header barmigarde
                return new Exception(httpResponseMessage.ReasonPhrase);
            }
        }

        public static void SetHeaders(
            HttpClient httpClient,
            List<KeyValuePair<string, string>> headers,
            bool validateHeaders
            )
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if (validateHeaders)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    else
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }
            }
        }

        public static void SetAcceptHeader(
            HttpClient httpClient,
            MediaTypeWithQualityHeaderValue accept
            )
        {
            if (accept == null)
            {
                // Add an Accept header for JSON format.
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            else
            {
                httpClient.DefaultRequestHeaders.Accept.Add(accept);
            }
        }

        public static void SetHttpClientTimeOut(
            HttpClient httpClient,
            TimeSpan? timeout
            )
        {
            if (timeout.HasValue)
            {
                httpClient.Timeout = timeout.Value;
            }
        }

        public static HttpClientHandler GetHttpClientHandler(
            CookieCollection cookies,
            string url
            )
        {
            CookieContainer cookieContainer = new CookieContainer();

            if (cookies != null)
            {
                Uri uri = new Uri(url);
                foreach (Cookie cookie in cookies)
                {
                    cookie.Domain = uri.Host;
                    cookieContainer.Add(cookie);
                }
            }

            return new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                CookieContainer = cookieContainer
            };
        }
    }
}