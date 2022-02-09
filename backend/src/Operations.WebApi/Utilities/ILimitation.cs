using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.WebApi.Utilities
{
    public interface ILimitation
    {
        [Range(1, int.MaxValue)]
        public int? Limit { get; set; }

        [Range(0, int.MaxValue)]
        public int? Offset { get; set; }
    }
}
