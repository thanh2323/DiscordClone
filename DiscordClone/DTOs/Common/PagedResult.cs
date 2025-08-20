namespace DiscordClone.DTOs.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }

        public PagedResult(IEnumerable<T> data, int totalCount, int page, int pageSize)
        {
            Data = data;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            HasNextPage = Page < TotalPages;
            HasPreviousPage = Page > 1;
        }
    }
}
