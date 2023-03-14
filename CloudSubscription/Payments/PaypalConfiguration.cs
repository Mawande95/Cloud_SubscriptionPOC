using PayPal.Api;

namespace CloudSubscriptionWeb.Payments
{
    public static class PaypalConfiguration
    {
        static PaypalConfiguration()
        {

        }
        public static Dictionary<string, string> GetConfig(string mode)
        {
            return new Dictionary<string, string>()
            {
                { "mode", mode }
            };
        }
        private static string GetAccessToken(string ClientId,string ClientSecret,string mode)
        {
            string accessToken =new OAuthTokenCredential(ClientId,ClientSecret, new Dictionary<string, string>()
            {
                { "mode", mode }
            }).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext(string clientId,string ClientSecret,string mode)
        {
            APIContext APIContext= new APIContext(GetAccessToken(clientId, ClientSecret, mode));
            APIContext.Config = GetConfig(mode);
            return APIContext;
        }
    }
}
