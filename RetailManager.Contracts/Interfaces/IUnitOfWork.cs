using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace RetailManager.Contracts.Interfaces;

public interface IUnitOfWork : IAsyncDisposable, IDisposable
{
    DbConnection DbConnection { get; }

    Task<bool> TransactionOpened { get; }

    DbTransaction Transaction { get; }

    Task BeginTransactionAsync(IsolationLevel transactionIsolationLevel =
        IsolationLevel.ReadCommitted);

    void BeginTransaction(IsolationLevel transactionIsolationLevel =
        IsolationLevel.ReadCommitted);

    Task CommitTransactionAsync();

    void CommitTransaction();

    Task RollbackTransactionAsync();

    void RollbackTransaction();
}
