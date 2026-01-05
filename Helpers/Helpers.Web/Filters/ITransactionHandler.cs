using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;

namespace Helpers.Web.Filters;

public interface ITransactionHandler
{
    Task WrapInTransactionAsync(ActionExecutionDelegate next, IsolationLevel isolationLevel);
}