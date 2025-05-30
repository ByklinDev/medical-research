using FluentValidation;
using MedicalResearch.Api.DTO;

namespace MedicalResearch.Api.DTOValidators
{
    public class QueryDTOValidator<T>: AbstractValidator<QueryDTO>
    {
        public QueryDTOValidator() 
        {
            RuleFor(x => x.Take)
                .GreaterThan(0)
                .WithMessage("Take must be greater than 0.");

            RuleFor(x => x.Skip)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Skip must be greater than or equal to 0.");
            
            RuleFor(x => x.SortColumn)
               .Must(w => string.IsNullOrEmpty(w) || typeof(T).GetProperty(w) != null)
                .WithMessage($"SortColumn must be a valid property of {typeof(T).Name}.");

            RuleFor(x => x.SearchTerm)
                .Must(w => string.IsNullOrEmpty(w) || w.Length >= 3)
                .WithMessage("SearchTerm must be at least 3 characters long if provided.");
        }
    }
}
