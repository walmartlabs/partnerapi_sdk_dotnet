using Newtonsoft.Json;

namespace Walmart.Sdk.Marketplace.V3.Payload.Token
{
    public class TokenType
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string Token { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
