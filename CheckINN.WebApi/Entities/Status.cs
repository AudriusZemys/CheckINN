using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace CheckINN.WebApi.Entities
{
    public class Status
    {
        public Status(int code, string message)
        {
            Code = code;
            Message = message;
        }

        [JsonProperty("code")]
        public int Code { get; }
        [JsonProperty("messsage")]
        public string Message { get; }
    }
}
