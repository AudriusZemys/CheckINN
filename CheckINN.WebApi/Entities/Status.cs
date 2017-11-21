using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace CheckINN.WebApi.Entities
{
    public class Status
    {
        public Status(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        [JsonProperty("success")]
        public bool Success { get; }
        [JsonProperty("messsage")]
        public string Message { get; }
    }
}
