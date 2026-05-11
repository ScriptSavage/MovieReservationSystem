using System.Data;
using Domain.Abstractions;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _context;

    public UnitOfWork(DatabaseContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        return transaction.GetDbTransaction();
    }
}