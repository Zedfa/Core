using Core.Cmn.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public class HttpHelper
    {
        public static string WebApiPut(string url, List<KeyValuePair<string, string>> headers, string content)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };
            using (HttpClient httpClient = new HttpClient(handler))
            {
                // Add an Accept header for JSON format.
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
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
                    return result;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        throw (new Exception(result));
                    }
                    else
                    {
                        /// chon ye vaght haee (mesle Api caspian) error tavasote header barmigarde
                        throw (new Exception(httpResponseMessage.ReasonPhrase));
                    }
                }
            }
        }
        public static T WebApiPut<T>(string url, List<KeyValuePair<string, string>> headers, string content)
        {
            var result = WebApiPut(url, headers, content);
            return result.DeSerializeJSONToObject<T>();
        }
        public static T WebApiPut<T>(string url, string content)
        {
            var result = WebApiPut(url, null, content);
            return result.DeSerializeJSONToObject<T>();
        }
        public static string WebApiPut(string url, string content)
        {
            return WebApiPut(url, null, content);
        }

        public static string WebApiPost(string url, List<KeyValuePair<string, string>> headers, HttpContent content)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };
            using (HttpClient httpClient = new HttpClient(handler))
            {
                // Add an Accept header for JSON format.
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                Task<HttpResponseMessage> httpResponseMessageTask = httpClient.PostAsync(url, content);
                HttpResponseMessage httpResponseMessage = httpResponseMessageTask.Result;
                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return result;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        throw (new Exception(result));
                    }
                    else
                    {
                        /// chon ye vaght haee (mesle Api caspian) error tavasote header barmigarde
                        throw (new Exception(httpResponseMessage.ReasonPhrase));
                    }
                }
            }
        }
        public static string WebApiPost(string url, List<KeyValuePair<string, string>> headers, string content)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };
            using (HttpClient httpClient = new HttpClient(handler))
            {
                // Add an Accept header for JSON format.
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                Task<HttpResponseMessage> httpResponseMessageTask = httpClient.PostAsync(
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
                    return result;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        throw (new Exception(result));
                    }
                    else
                    {
                        /// chon ye vaght haee (mesle Api caspian) error tavasote header barmigarde
                        throw (new Exception(httpResponseMessage.ReasonPhrase));
                    }
                }
            }
        }
        public static T WebApiPost<T>(string url, List<KeyValuePair<string, string>> headers, HttpContent content)
        {
            var result = WebApiPost(url, headers, content);
            return result.DeSerializeJSONToObject<T>();
        }
        public static T WebApiPost<T>(string url, List<KeyValuePair<string, string>> headers, string content)
        {
            var result = WebApiPost(url, headers, content);
            return result.DeSerializeJSONToObject<T>();
        }
        public static T WebApiPost<T>(string url, HttpContent content)
        {
            var result = WebApiPost(url, null, content);
            return result.DeSerializeJSONToObject<T>();
        }
        public static T WebApiPost<T>(string url, string content)
        {
            var result = WebApiPost(url, null, content);
            return result.DeSerializeJSONToObject<T>();
        }
        public static string WebApiPost(string url, HttpContent content)
        {
            return WebApiPost(url, null, content);
        }
        public static string WebApiPost(string url, string content)
        {
            return WebApiPost(url, null, content);
        }

        public static string WebApiGet(string url, List<KeyValuePair<string, string>> headers)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };
            using (HttpClient httpClient = new HttpClient(handler))
            {
                // Add an Accept header for JSON format.
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                Task<HttpResponseMessage> httpResponseMessageTask = httpClient.GetAsync(url);
                HttpResponseMessage httpResponseMessage = httpResponseMessageTask.Result;
                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return result;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        throw (new Exception(result));
                    }
                    else
                    {
                        /// chon ye vaght haee (mesle Api caspian) error tavasote header barmigarde
                        throw (new Exception(httpResponseMessage.ReasonPhrase));
                    }
                }
            }
        }
        public static T WebApiGet<T>(string url, List<KeyValuePair<string, string>> headers)
        {
            var content = WebApiGet(url, headers);
            return content.DeSerializeJSONToObject<T>();
            //return (T)Convert.ChangeType(content, typeof(T));
        }
        public static T WebApiGet<T>(string url)
        {
            var content = WebApiGet(url, null);
            return content.DeSerializeJSONToObject<T>();
            //return (T)Convert.ChangeType(content, typeof(T));
        }
        public static string WebApiGet(string url)
        {
            return WebApiGet(url, null);
        }

        public static string WebApiDelete(string url, List<KeyValuePair<string, string>> headers)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };
            using (HttpClient httpClient = new HttpClient(handler))
            {
                // Add an Accept header for JSON format.
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                Task<HttpResponseMessage> httpResponseMessageTask = httpClient.DeleteAsync(url);
                HttpResponseMessage httpResponseMessage = httpResponseMessageTask.Result;
                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return result;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        throw (new Exception(result));
                    }
                    else
                    {
                        /// chon ye vaght haee (mesle Api caspian) error tavasote header barmigarde
                        throw (new Exception(httpResponseMessage.ReasonPhrase));
                    }
                }
            }
        }
        public static T WebApiDelete<T>(string url, List<KeyValuePair<string, string>> headers)
        {
            var result = WebApiDelete(url, headers);
            return result.DeSerializeJSONToObject<T>();
        }
        public static T WebApiDelete<T>(string url)
        {
            var result = WebApiDelete(url, null);
            return result.DeSerializeJSONToObject<T>();
        }
        public static string WebApiDelete(string url)
        {
            return WebApiDelete(url, null);
        }
    }
}
