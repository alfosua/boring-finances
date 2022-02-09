using System.Text.Json.Serialization;

namespace BoringFinances.Operations.WebApi.Utilities
{
    public class CountedEnumerable<T>
    {
        [JsonIgnore]
        public ILimitation? Limitation { get; set; }

        public IEnumerable<T> Items { get; set; } = new List<T>();

        public int Count => Items.Count();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? FilteredCount { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TotalCount { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Offset => Limitation?.Offset;
    }
}
