using System.Reflection;
using System.Text;

namespace Utilities.QueryBuilder
{
    public static class OrderQueryBuilder
    {
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString)) return string.Empty;

            string[] orderParams = orderByQueryString.Trim().Split(',');
            PropertyInfo[] propertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            StringBuilder orderQueryBuilder = new();

            foreach (string param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param)) continue;

                string[] parts = param.Trim().Split(' ');
                string propertyPath = parts[0];
                string direction = (parts.Length > 1 && parts[1].Equals("desc", StringComparison.InvariantCultureIgnoreCase)) ? "descending" : "ascending";

                
                if (IsPropertyValid(typeof(T), propertyPath))
                {
                    _ = orderQueryBuilder.Append($"{propertyPath} {direction},");
                }
                else if (propertyPath.Contains('.'))
                {
                    string strippedPath = string.Join(".", propertyPath.Split('.').Skip(1));
                    if (IsPropertyValid(typeof(T), strippedPath))
                    {
                        _ = orderQueryBuilder.Append($"{strippedPath} {direction},");
                    }
                }
            }

            return orderQueryBuilder.ToString().TrimEnd(',', ' ');
        }

        private static bool IsPropertyValid(Type type, string propertyPath)
        {
            Type currentType = type;
            foreach (var part in propertyPath.Split('.'))
            {
                var prop = currentType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(pi => pi.Name.Equals(part, StringComparison.InvariantCultureIgnoreCase));
                if (prop == null) return false;
                currentType = prop.PropertyType;
            }
            return true;
        }
    }
}
