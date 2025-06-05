using Microsoft.AspNetCore.Mvc.Filters;
using MedicalResearch.Domain.Extensions;

namespace MedicalResearch.Api.Filters;

public class PageListFilterAttribute<T>: Attribute, IResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context)
    {
        
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is not Microsoft.AspNetCore.Mvc.ObjectResult objectResult)
        {
            return;
        }
        if (objectResult.Value is not PagedList<T> pagedList)
        {
            return;
        }
        var pagedata = new
        {
            pagedList.TotalCount,
            pagedList.PageSize,
            pagedList.CurrentPage,
            pagedList.TotalPages,
            pagedList.HasNext,
            pagedList.HasPrevious
        };
        context.HttpContext.Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(pagedata));
    }
}
