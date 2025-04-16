namespace crudmysql.DTOs
{
    public class PaginatedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int From => (PageNumber - 1) * PageSize + 1;
        public int To => Math.Min(PageNumber * PageSize, TotalCount);
        public List<T> Items { get; set; }
    }

}
