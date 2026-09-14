using Newtonsoft.Json;
using Utilities.Extensions;
using Utilities.RequestHandler;

namespace Utilities.PaginationHelper
{
    public class MetaData : PaginationMetaData
    {
        public static string PaginationMetaData(MetaData metaData, RequestParameters requestParameters, string actionUri)
        {
            metaData.PreviousPageLink = metaData.HasPrevious ? UrlLink(requestParameters.PageNumber - 1, requestParameters.PageSize, requestParameters.OrderBy, requestParameters.SearchTerm, actionUri) : null;
            metaData.NextPageLink = metaData.HasNext ? UrlLink(requestParameters.PageNumber + 1, requestParameters.PageSize, requestParameters.OrderBy, requestParameters.SearchTerm, actionUri) : null;

            return JsonConvert.SerializeObject(metaData).CommaEncode();
        }

        public static string PaginationMetaData()
        {
            return JsonConvert.SerializeObject(new MetaData()).CommaEncode();
        }

        private static string UrlLink(int pageNumber, int pageSize, string orderBy, string searchTerm, string actionUri)
        {
            return $"{actionUri}?orderBy={orderBy}&searchTerm={searchTerm}&pageNumber={pageNumber}&pageSize={pageSize}";
        }
    }
}
