using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;

namespace Helpers.Web.Filters;

public class TransactionalAttribute : TypeFilterAttribute
{
    public TransactionalAttribute(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) 
        : base(typeof(TransactionalActionFilter))
    {
        Arguments = [isolationLevel];
    }
}

public class TransactionalActionFilter : IAsyncActionFilter
{
    private readonly ITransactionHandler _transactionManager;
    private readonly IsolationLevel _isolationLevel;

    public TransactionalActionFilter(
        ITransactionHandler transactionManager,
        IsolationLevel isolationLevel)
    {
        _transactionManager = transactionManager;
        _isolationLevel = isolationLevel;
    }

    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) 
        => _transactionManager.WrapInTransactionAsync(next, _isolationLevel);
}
