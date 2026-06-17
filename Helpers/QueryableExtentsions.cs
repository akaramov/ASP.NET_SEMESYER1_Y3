using Microsoft.EntityFrameworkCore;

namespace APAERMENT_LAST_API.Helpers
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize)
        {
            var totalRecords = await query.CountAsync();

            var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<T>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = data
            };
        }
    }
}
