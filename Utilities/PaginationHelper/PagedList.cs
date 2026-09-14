using Microsoft.EntityFrameworkCore;

namespace Utilities.PaginationHelper
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        public PagedList(List<T> items, int pageNumber, int pageSize, int count)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            };
            AddRange(items);
        }

        public static async Task<PagedList<T>> ToPagedListAsync(IQueryable<T> source, int pageNumber, int pageSize, int? dataCount = null)
        {
            int count = dataCount ?? await source.CountAsync();

            List<T> items = await source.Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            return new PagedList<T>(items, pageNumber, pageSize,count);
        }
    }

    public static class DbSetExtensions
    {
        public static async Task<PagedList<T>> ToPagedListAsync<T>(
            this DbSet<T> source, int pageNumber, int pageSize, int? dataCount = null) where T : class
        {
            int count = dataCount ?? await source.CountAsync();

            List<T> items = await source.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PagedList<T>(items, pageNumber, pageSize, count);
        }
    }
}
