using MediatR;
using Microsoft.EntityFrameworkCore;
using YellowHouseStudio.LifeOrbit.Application.Data;

namespace YellowHouseStudio.LifeOrbit.Application.Common.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ApplicationDbContext _dbContext;

    public TransactionBehavior(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Skip if it's not a command (i.e., if it's a query)
        if (request is not IRequest<TResponse>) return await next();

        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var response = await next();
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return response;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
} 