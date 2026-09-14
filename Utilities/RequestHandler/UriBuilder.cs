namespace Utilities.RequestHandler
{
    public static class UriBuilder
    {
        public static string GetBaseUri(this string uri, string groupName)
        {
            if (!string.IsNullOrWhiteSpace(uri) && !string.IsNullOrWhiteSpace(groupName))
            {
                uri = uri.Split([groupName], StringSplitOptions.None)[0];
            }

            return uri;
        }
    }
}
