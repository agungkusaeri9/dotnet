namespace crudmysql.DTOs.Pagination
{
    public class PaginationInfo
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }

}
