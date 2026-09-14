namespace Utilities.Settings
{
    public class AppSettings
    {
        public string? AppKey { get; set; }
        public int RefreshTokenTTL { get; set; }
        public int TokenTTL { get; set; }
        public int VerificationTTL { get; set; }
        public string? Secret { get; set; }
    }
}
