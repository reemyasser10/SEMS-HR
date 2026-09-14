using System.Globalization;

namespace Utilities.RequestHandler
{
    public static class QueryParameterHelper
    {
        public static Dictionary<string, string> ToQueryParameters<T>(T parameters)
        {
            if (parameters == null) return [];

            return parameters.GetType()
                .GetProperties()
                .Where(p => p.GetValue(parameters) != null)
                .ToDictionary(
                    p => p.Name,
                    p =>
                    {
                        var value = p.GetValue(parameters);
                        if (value is System.Collections.IEnumerable enumerable && value is not string)
                        {
                            var items = enumerable.Cast<object>().Select(x => x.ToString());
                            return string.Join(",", items);
                        }else if (value is DateTime) {
                            return ((DateTime)value).ToString("o", CultureInfo.InvariantCulture);
                        }
                        return value?.ToString() ?? string.Empty;
                    }
                );
        }
    }

}
