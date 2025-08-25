namespace DiscordClone.DTOs.Common
{
    public class PaginationParams
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;

        public int Skip => (Page - 1) * PageSize;

        public PaginationParams()
        {
            Page = Page < 1 ? 1 : Page;
            PageSize = PageSize > 100 ? 100 : PageSize;
            PageSize = PageSize < 1 ? 50 : PageSize;
        }
    }

}