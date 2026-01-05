using Helpers.Common.Models;
using Helpers.Common.Validations;
using Helpers.Web.Filters;
using Helpers.Web.Middlewares.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Helpers.Web.Controllers;

public abstract class BaseCrudsController<TFilters, TResponse, TDetailedResponse, TRequest> : 
    ControllerBase, IHttpCrudsAsync<TFilters, TResponse, TDetailedResponse, TRequest>
    where TFilters : IValidate, IHttpRequestFilter
    where TResponse : class
    where TDetailedResponse : class
    where TRequest : class, IValidate, new()
{
    private readonly ILogger _logger;
    private readonly IApplicationService<TFilters, TResponse, TDetailedResponse, TRequest> _svc;
    private readonly ITranslationHandler _translationHandler;

    private BaseCrudsController(
        ILogger logger,
        IApplicationService<TFilters, TResponse, TDetailedResponse, TRequest> svc,
        ITranslationHandler translationHandler)
    {
        _logger = logger;
        _svc = svc;
        _translationHandler = translationHandler;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDescription))]
    [Transactional]
    public async Task<IActionResult> DeleteAsync([FromRoute] ulong id)
    {
        await _svc.DeleteAsync(id);
        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDescription))]
    public async Task<ActionResult<Pagination<TResponse>>> GetListAsync([FromQuery] TFilters filters)
    {
        filters.ValidateAndThrow(_translationHandler);
        return Ok(await _svc.GetListAsync(filters));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDescription))]
    public async Task<ActionResult<TDetailedResponse>> GetOneAsync([FromRoute] ulong id) => 
        Ok(await _svc.GetOneAsync(id));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDescription))]
    [Transactional]
    public async Task<IActionResult> PostAsync([FromBody] TRequest request)
    {
        request.ValidateAndThrow(_translationHandler);
        await _svc.CreateAsync(request);
        return StatusCode(StatusCodes.Status201Created); // TODO: Manage gateway URL
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDescription))]
    [Transactional]
    public async Task<IActionResult> PutAsync([FromRoute] ulong id, [FromBody] TRequest request)
    {
        request.ValidateAndThrow(_translationHandler);
        await _svc.UpdateAsync(id, request);
        return Ok();
    }
}