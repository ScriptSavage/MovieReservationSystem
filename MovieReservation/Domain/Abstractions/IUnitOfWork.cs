using System.Data;

namespace Domain.Abstractions;

public interface IUnitOfWork 
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
}