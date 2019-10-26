namespace ttn.TouTiao.Models
{
    public class GetTokenResponse // : TouTiaoResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
}
