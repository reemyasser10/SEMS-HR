namespace HR.Application.Common.Extensions;

using HR.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        return await query.ToPagedResultAsync(request.PageNumber, request.PageSize, cancellationToken);
    }

    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, pageNumber, pageSize);
    }

    public static async Task<PagedResult<TDestination>> ToPagedResultAsync<TSource, TDestination>(
        this IQueryable<TSource> query,
        PaginationRequest request,
        Expression<Func<TSource, TDestination>> projection,
        CancellationToken cancellationToken = default)
    {
        return await query.ToPagedResultAsync(request.PageNumber, request.PageSize, projection, cancellationToken);
    }

    public static async Task<PagedResult<TDestination>> ToPagedResultAsync<TSource, TDestination>(
        this IQueryable<TSource> query,
        int pageNumber,
        int pageSize,
        Expression<Func<TSource, TDestination>> projection,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(projection)
            .ToListAsync(cancellationToken);

        return new PagedResult<TDestination>(items, totalCount, pageNumber, pageSize);
    }
}
