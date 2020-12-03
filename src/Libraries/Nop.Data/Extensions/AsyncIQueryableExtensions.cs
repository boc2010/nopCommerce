using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB;
using Nop.Core;

namespace Nop.Data.Extensions
{
    public static class AsyncIQueryableExtensions
    {
        public static ValueTask<TSource> GetFirstOrDefaultAsync<TSource>(this IQueryable<TSource> query,
            Expression<Func<TSource, bool>> predicate = null)
        {
            return new ValueTask<TSource>(predicate == null ? query.FirstOrDefaultAsync() : query.FirstOrDefaultAsync(predicate));
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, bool getOnlyTotalCount = false)
        {
            if (source == null)
                return new PagedList<T>(new List<T>(), pageIndex, pageSize);

            var count = await source.CountAsync();

            var data = new List<T>();

            if (!getOnlyTotalCount)
                data.AddRange(await source.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync());

            return new PagedList<T>(data, pageIndex, pageSize, count);
        }
    }

    public static class AsyncIEnumerableExtensions
    {
        public static IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, ValueTask<TResult>> selector)
        {
            return source.ToAsyncEnumerable().SelectAwait(selector);
        }
    }

    
}
