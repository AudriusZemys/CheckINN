using Newtonsoft.Json;

namespace CheckINN.WebApi.Entities
{
    public class User
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
