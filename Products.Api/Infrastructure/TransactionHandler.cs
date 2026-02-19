using Helpers.Web.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
using Products.Infrastructure.Data;
using System.Data;

namespace Products.Api.Infrastructure;

public class TransactionHandler : ITransactionHandler
{
    private readonly ProductsDbContext _dbContext;

    public TransactionHandler(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task WrapInTransactionAsync(ActionExecutionDelegate next, IsolationLevel isolationLevel)
    {
        IDbContextTransaction? transaction = null;
        try
        {
            transaction = await _dbContext.Database.BeginTransactionAsync();
            var result = await next();
            
            if (result.Exception == null)
            {
                await transaction.CommitAsync();
            }
            else
            {
                await transaction.RollbackAsync();
            }
        }
        catch
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
        finally
        {
            transaction?.Dispose();
        }
    }
}
