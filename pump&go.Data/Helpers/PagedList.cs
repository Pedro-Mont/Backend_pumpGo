using Microsoft.EntityFrameworkCore;

namespace pump_go.pump_go.Data.Helpers
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var totalCount = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, totalCount);
        }
    }
}
