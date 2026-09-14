using Newtonsoft.Json;
using System.Text;
using Utilities.Extensions;

namespace Utilities.ResponseHandler
{
    public class ResponseStatus
    {
        public ResponseStatus()
        {
        }

        public ResponseStatus(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; } = "";

        public string PlainErrorMessage { get; set; } = "";

        public string ExceptionMessage { get; set; } = "";

        public override string ToString()
        {
            ErrorMessage = ErrorMessage.ToBase64();
            return JsonConvert.SerializeObject(this).CommaEncode();
        }
    }
}
