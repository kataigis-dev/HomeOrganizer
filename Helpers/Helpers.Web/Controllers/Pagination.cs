namespace Helpers.Web.Controllers;

public record Pagination<TResponse>(List<TResponse> Content, uint Page, uint PageCount, uint PageSize) 
    where TResponse : class;