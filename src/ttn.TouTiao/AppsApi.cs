using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ttn.TouTiao.Models;
using static System.FormattableString;

namespace ttn.TouTiao
{
    public static class AppsApi
    {
        static IHttpClient httpClient;
        static IJsonSerializerService jsonSerializerService;

        /// <summary>
        /// https://developer.toutiao.com/docs/server/auth/accessToken.html
        /// </summary>
        /// <param name="appid">小程序ID</param>
        /// <param name="secret">小程序的 APP Secret，可以在开发者后台获取</param>
        /// <param name="grant_type">获取 access_token 时值为 client_credential</param>
        /// <returns></returns>
        public static async Task<TouTiaoResponse<GetTokenResponse>> GetAccessTokenAsync(string appid, string secret, string grant_type = "client_credential")
        {
            var requestString = $"{Consts.ApiHost}/apps/token?appid={appid}&secret={secret}&grant_type={grant_type}";

            httpClient = httpClient ?? new ZipHttpClient();
            var response = await httpClient.HttpRequestAsync($"{requestString}").ConfigureAwait(false);
            var responseContent = response.Content?.ReadAsStringAsync();

            TouTiaoResponse<GetTokenResponse> res = new TouTiaoResponse<GetTokenResponse>()
            {
                IsSuccessStatus = response.IsSuccessStatusCode
            };
            if (!res.IsSuccessStatus)
            {
                return null;
            }

            jsonSerializerService = jsonSerializerService
                ?? new JsonNetJsonSerializerService();

            try
            {
                res.Response =
                    await jsonSerializerService.DeserializeJsonAsync<GetTokenResponse>(responseContent).ConfigureAwait(false);
            }
            catch (FormatException e)
            {
                res.Response = null;
                res.IsSuccessStatus = false;
                res.ResponseReasonPhrase = $"Error parsing results: {e?.InnerException?.Message ?? e.Message}";
            }

            if (null == res.Response || String.IsNullOrEmpty(res.Response.access_token))
            {
                res.Error =
                    await jsonSerializerService.DeserializeJsonAsync<TouTiaoError>(responseContent).ConfigureAwait(false);
            } else
            {
                response.Headers.TryGetValues("x-tt-timestamp", out var responseTimeHeader);

                res.Headers = new ResponseHeaders
                {
                    CacheControl = response.Headers.CacheControl,
                    ResponseTime = responseTimeHeader?.FirstOrDefault()
                };
            }

            return res;
        }
    }
}
