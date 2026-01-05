using Microsoft.AspNetCore.Mvc;

namespace Helpers.Web.Controllers;

public interface IHttpCrudsAsync<TFilters, TResponse, TDetailedResponse, TRequest> : 
    IHttpGetListAsync<TFilters, TResponse>, 
    IHttpGetOneAsync<TDetailedResponse>, 
    IHttpPostAsync<TRequest>, 
    IHttpPutAsync<TRequest>, 
    IHttpDeleteAsync
    where TFilters : IHttpRequestFilter
    where TResponse : class
    where TDetailedResponse : class
    where TRequest : class, new() { }

public interface IHttpGetListAsync<TFilters, TResponse>
    where TFilters : IHttpRequestFilter
    where TResponse : class
{
    Task<ActionResult<Pagination<TResponse>>> GetListAsync([FromQuery] TFilters filters);
}

public interface IHttpGetOneAsync<TDetailedResponse>
    where TDetailedResponse : class
{
    Task<ActionResult<TDetailedResponse>> GetOneAsync([FromRoute] ulong id);
}

public interface IHttpPostAsync<TRequest>
    where TRequest : class, new()
{
    Task<IActionResult> PostAsync([FromBody] TRequest request);
}

public interface IHttpPutAsync<TRequest>
    where TRequest : class, new()
{
    Task<IActionResult> PutAsync([FromRoute] ulong id, [FromBody] TRequest request);
}

public interface IHttpDeleteAsync
{
    Task<IActionResult> DeleteAsync([FromRoute] ulong id);
}