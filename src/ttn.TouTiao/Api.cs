using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ttn.TouTiao.Models;

namespace ttn.TouTiao
{
    public static class Api
    {
        static IHttpClient httpClient;
        static IJsonSerializerService jsonSerializerService;

        public static async Task<TouTiaoResponse<T>> GetAsync<T>(string requestString) where T : class
        {
            httpClient = httpClient ?? new ZipHttpClient();
            var response = await httpClient.GetAsync($"{requestString}").ConfigureAwait(false);
            return await ParseContent<T>(response);
        }

        public static async Task<TouTiaoResponse<T>> PostJsonBodyAsync<T>(string requestString, object obj) where T : class
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(obj));
            return await PostAsync<T>(requestString, content);
        }

        public static async Task<TouTiaoResponse<T>> PostFormAsync<T>(string requestString, List<KeyValuePair<String, String>> paramList) where T : class
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(paramList);
            return await PostAsync<T>(requestString, content);
        }

        public static async Task<TouTiaoResponse<T>> PostAsync<T>(string requestString, HttpContent content) where T : class
        {
            httpClient = httpClient ?? new ZipHttpClient();
            var response = await httpClient.PostAsync($"{requestString}", content).ConfigureAwait(false);
            return await ParseContent<T>(response);
        }

        private static async Task<TouTiaoResponse<T>> ParseContent<T>(HttpResponseMessage response) where T : class
        {
            TouTiaoResponse<T> res = new TouTiaoResponse<T>()
            {
                IsSuccessStatus = response.IsSuccessStatusCode
            };

            if (!res.IsSuccessStatus)
            {
                return null;
            }

            var contentType = response.Content.Headers.ContentType;

            switch (contentType.MediaType)
            {
                case "application/json":
                    break;
                default:
                    res.Response = response.Content as T;
                    return res;
            }

            Task<String> responseContent = response.Content?.ReadAsStringAsync();
            jsonSerializerService = jsonSerializerService
                ?? new JsonNetJsonSerializerService();
            
            try
            {
                res.Error =
                    await jsonSerializerService.DeserializeJsonAsync<TouTiaoError>(responseContent).ConfigureAwait(false);

                if (res.Error.errcode == 0 && String.IsNullOrEmpty(res.Error.errmsg))
                {
                    res.Error = null;
                }

                if (null == res.Error)
                {
                    res.Response =
                    await jsonSerializerService.DeserializeJsonAsync<T>(responseContent).ConfigureAwait(false);

                    if (null != res.Response)
                    {
                        res.IsSuccessStatus = true;
                    }
                }
            }
            catch (FormatException e)
            {
                res.Response = null;
                res.IsSuccessStatus = false;
                res.ResponseReasonPhrase = $"Error parsing results: {e?.InnerException?.Message ?? e.Message}";
            }

            if (null != res.Response)
            {
                response.Headers.TryGetValues("x-tt-timestamp", out var responseTimeHeader);               
                res.Headers = new ResponseHeaders
                {
                    CacheControl = response.Headers.CacheControl,
                    ResponseTime = responseTimeHeader?.FirstOrDefault(),
                    ContentType = response.Content.Headers.ContentType
                };
            }

            return res;
        }
    }
}
