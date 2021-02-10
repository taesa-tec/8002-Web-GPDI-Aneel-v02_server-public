using System.Collections.Generic;

namespace PeD.Core
{
    public class Pagination<T>
    {
        public List<T> Data { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        public List<PaginationFilter> Filters { get; set; }
    }

    public class PaginationFilter
    {
        public string Name { get; set; }
        public string Field { get; set; }
        public Dictionary<string, string> Values { get; set; }
    }
}