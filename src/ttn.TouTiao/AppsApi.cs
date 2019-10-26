using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ttn.TouTiao.Models;
using static System.FormattableString;

namespace ttn.TouTiao
{
    public enum TTApp
    {
        toutiao,
        douyin,
        pipixia,
        huoshan
    }

    public static class AppsApi
    {
        /// <summary>
        /// Get Access Token
        /// </summary>
        /// <param name="appid">小程序ID</param>
        /// <param name="secret">小程序的 APP Secret，可以在开发者后台获取</param>
        /// <param name="grant_type">获取 access_token 时值为 client_credential</param>
        /// <remarks>全局唯一调用凭据</remarks>
        /// <see cref="https://developer.toutiao.com/docs/server/auth/accessToken.html"/>
        /// <returns></returns>
        public static async Task<TouTiaoResponse<GetTokenResponse>> GetAccessTokenAsync(string appid, string secret, string grant_type = "client_credential")
        {
            var requestString = $"{Consts.ApiHost}/apps/token?appid={appid}&secret={secret}&grant_type={grant_type}";

            return await Api.GetAsync<GetTokenResponse>(requestString);
        }

        /// <summary>
        /// Create QRCode
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="appname">是打开二维码的字节系app名称，默认为今日头条，取值 toutiao	今日头条; douyin 抖音; pipixia 皮皮虾; huoshan 火山小视频</param>
        /// <param name="path">小程序/小游戏启动参数，小程序则格式为encode({path}?{query})，小游戏则格式为JSON字符串，默认为空</param>
        /// <param name="width"></param>
        /// <param name="line_color">二维码线条颜色，默认为黑色 {"r":0,"g":0,"b":0}</param>
        /// <param name="background">二维码背景颜色，默认为透明</param>
        /// <param name="set_icon">是否展示小程序/小游戏icon，默认不展示</param>
        /// <see cref="https://developer.toutiao.com/docs/server/qrcode/qrcode.html"/>
        /// <returns></returns>
        public static async Task<TouTiaoResponse<HttpContent>> CreateQRCodeAsync(string accessToken, TTApp appname = TTApp.toutiao, string path = "", int width = 430, bool set_icon = false
            //, string line_color = "", string background = ""
            )
        {
            var requestString = $"{Consts.ApiHost}/apps/qrcode";
            var obj = new
            {
                access_token = accessToken,
                appname = appname.ToString(),
                path = path,
                width = width,
                //line_color = line_color,
                //background = background,
                set_icon = set_icon
            };

            //var content = $"\"access_token\": \"{accessToken}\",\"appname\":\"{appname}\"";

            return await Api.PostJsonBodyAsync<HttpContent>(requestString, obj);
        }
    }
}
