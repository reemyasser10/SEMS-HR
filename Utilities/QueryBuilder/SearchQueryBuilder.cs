using System.Linq.Expressions;
using System.Reflection;

namespace Utilities.QueryBuilder
{
    public static class SearchQueryBuilder
    {
        public static Expression<Func<T, bool>> CreateSearchQuery<T>(string searchColumns, string searchTerm)
        {
            string[] columnNames = searchColumns.Trim().Split(',');

            ParameterExpression row = Expression.Parameter(typeof(T), "row");
            Expression? body = null;
            Expression searchVal = Expression.Constant(searchTerm, typeof(string));

            PropertyInfo[] propertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (string columnName in columnNames)
            {
                if (string.IsNullOrWhiteSpace(columnName))
                {
                    continue;
                }

                string propertyFromQueryName = columnName.Split(" ")[0];
                PropertyInfo? objectProperty = propertyInfo.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                {
                    continue;
                }

                Expression col = Expression.PropertyOrField(row, objectProperty.Name);
                Expression colString = (col.Type == typeof(string)) ? col : Expression.Call(col, "ToString", null, null);
                Expression colSearch = Expression.Call(colString, "Contains", null, searchVal);

                if (col.Type.IsClass)
                {
                    colSearch = Expression.AndAlso(Expression.NotEqual(col, Expression.Constant(null, typeof(string))), colSearch);
                }
                else if (Nullable.GetUnderlyingType(col.Type) != null)
                {
                    // Nullable<T>
                    colSearch = Expression.AndAlso(Expression.Property(col, "HasValue"), colSearch);
                }
                body = (body == null) ? colSearch : Expression.OrElse(body, colSearch);
            }
            Expression<Func<T, bool>> predicate = (body == null) ? x => false : Expression.Lambda<Func<T, bool>>(body, row);

            return predicate;
        }
    }
}
