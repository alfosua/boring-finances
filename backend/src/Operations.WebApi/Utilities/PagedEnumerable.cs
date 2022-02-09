using System.Text.Json.Serialization;

namespace BoringFinances.Operations.WebApi.Utilities
{
    public class PagedEnumerable<T>
    {
        [JsonIgnore]
        public IPagination? Pagination { get; set; }

        public IEnumerable<T> Items { get; set; } = new List<T>();

        public int Count => Items.Count();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? FilteredCount { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TotalCount { get; set; }

        public int PageNumber => Pagination?.PageNumber ?? 1;

        public int PageSize => Pagination?.PageSize ?? 0;
    }
}
