using MedicalResearch.Domain.Constants;
using MedicalResearch.Domain.Models;

namespace MedicalResearch.Api.DTO
{
    public class QueryDTO
    {
        public int Take { get; set; } = AppConstants.PAGE_DEFAULT_SIZE;
        public int Skip { get; set; } = 0;
        public string? SortColumn { get; set; } = "Id";
        public string? SearchTerm { get; set; }
        public bool IsAscending { get; set; } = true;
    }
}
