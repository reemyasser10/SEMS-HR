using Newtonsoft.Json;
using System.Globalization;
using static Utilities.Extensions.StringExtensions;

namespace Identity.Entities
{
    public class TokenResponse
    {
        public string JwtToken { get; set; }

        public string Expires { get; set; }

        public TokenResponse() { }

        public TokenResponse(string token, DateTime expires)
        {
            JwtToken = token;
            Expires = expires.ToString("ddd, dd MMM yyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this).CommaEncode();
        }
    }
}
