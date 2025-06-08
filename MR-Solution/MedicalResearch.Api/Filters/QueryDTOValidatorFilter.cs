using FluentValidation;
using MedicalResearch.Api.DTO;
using MedicalResearch.Api.DTOValidators;
using MedicalResearch.Domain.Exceptions;
using MedicalResearch.Domain.Models;
using MedicalResearch.Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalResearch.Api.Filters;

public class QueryDTOValidatorFilter<T> : Attribute, IAsyncActionFilter where T : Entity
{
    private readonly IValidator<QueryDTO>? _validator;

    public QueryDTOValidatorFilter(IEnumerable<IValidator<QueryDTO>> validators) 
    {
        if (!validators.Any())
        {
            throw new DomainException("Not exist QueryDTO validators in DI");
        }
        _validator = validators.FirstOrDefault(v => v.GetType() == typeof(QueryDTOValidator<T>));
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Check if the action has a DTO parameter
        var dtoParameter = context.ActionArguments.Values.FirstOrDefault(v => v != null && v.GetType().Name.EndsWith("DTO"));
        if (dtoParameter != null)
        {
            if (_validator == null)
            {
                context.Result = new BadRequestObjectResult($"Validator for QueryDTO not found.");
                return;
            }
            var resultValidation = await _validator.ValidateAsync((QueryDTO)dtoParameter);
            if (!resultValidation.IsValid)
            {
                context.Result = new BadRequestObjectResult(resultValidation.Errors.Select(e => e.ErrorMessage));
                return;
            }      
        }
        // Proceed to the next action filter or action method
        await next();
    }
}
