namespace Helpers.Web.Controllers;

public interface IApplicationService<TFilters, TResponse, TDetailedResponse, TRequest>
    where TFilters : IHttpRequestFilter
    where TResponse : class
    where TDetailedResponse : class
    where TRequest : class, new()
{
    Task CreateAsync(TRequest request);
    Task DeleteAsync(ulong id);
    Task<Pagination<TResponse>> GetListAsync(TFilters filters);
    Task<TDetailedResponse> GetOneAsync(ulong id);
    Task UpdateAsync(ulong id, TRequest request);
}