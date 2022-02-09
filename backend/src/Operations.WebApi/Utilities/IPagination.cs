using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.WebApi.Utilities
{
    public interface IPagination
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }
    }
}
