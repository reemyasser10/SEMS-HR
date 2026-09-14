using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilities.QueryBuilder;
using System.Linq.Dynamic.Core;

namespace Utilities.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<T> Search<T>(this IQueryable<T> data, string searchColumns, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchColumns))
                return data;

            searchTerm = searchTerm.SafeTrim().SafeLower();

            Expression<Func<T, bool>> expression = SearchQueryBuilder.CreateSearchQuery<T>(searchColumns, searchTerm);

            return data.Where(expression);
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> data, string orderByQueryString) where T : class
        {

            if (string.IsNullOrEmpty(orderByQueryString))
            {
                return data;
            }
            string orderQuery = OrderQueryBuilder.CreateOrderQuery<T>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return data;
            }

            return data.OrderBy(orderQuery);
        }
    }
}
