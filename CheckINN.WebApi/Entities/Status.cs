using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace CheckINN.WebApi.Entities
{
    /// <summary>
    /// Basic status describing class, 
    /// can be used as a return value for any method that requires such info
    /// </summary>
    public class Status
    {
        public Status(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        /// <summary>
        /// Is the operation requested successful or not
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; }

        /// <summary>
        /// Associated message, shoud describe the error, 
        /// or carry a significant (or not) success message
        /// </summary>
        [JsonProperty("messsage")]
        public string Message { get; }
    }
}
