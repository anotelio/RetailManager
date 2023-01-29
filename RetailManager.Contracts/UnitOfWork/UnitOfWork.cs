using System;
using System.Data;
using RetailManager.Contracts.Interfaces;
using System.Threading.Tasks;
using System.Data.Common;
using System.Transactions;
using IsolationLevel = System.Data.IsolationLevel;

namespace RetailManager.Contracts.UnitOfWork;

public abstract class UnitOfWork : IUnitOfWork
{
    protected UnitOfWork(DbConnection dbConnection)
    {
        this.DbConnection = dbConnection;
    }

    public DbConnection DbConnection { get; }

    private bool Disposed { get; set; }

    public DbTransaction Transaction { get; private set; }

    public Task<bool> TransactionOpened => Task.FromResult(this.Transaction != null);

    public async Task BeginTransactionAsync(IsolationLevel transactionIsolationLevel = IsolationLevel.ReadCommitted)
    {
        if (this.Transaction is not null)
        {
            throw new NotImplementedException("Multiple transactions are not implemented.");
        }

        if (this.DbConnection.State != ConnectionState.Open)
        {
            await this.DbConnection.OpenAsync();
        }

        this.Transaction = await this.DbConnection.BeginTransactionAsync(transactionIsolationLevel);
    }

    public void BeginTransaction(IsolationLevel transactionIsolationLevel = IsolationLevel.ReadCommitted)
    {
        if (this.Transaction is not null)
        {
            throw new NotImplementedException("Multiple transactions are not implemented.");
        }

        if (this.DbConnection.State != ConnectionState.Open)
        {
            this.DbConnection.Open();
        }

        this.Transaction = this.DbConnection.BeginTransaction(transactionIsolationLevel);
    }

    public async Task CommitTransactionAsync()
    {
        if (this.Transaction is null)
        {
            throw new TransactionException("There is no open transaction to be committed.");
        }

        if (this.Transaction.Connection != null)
        {
            await this.Transaction.CommitAsync();
        }

        //await this.Transaction.DisposeAsync();
        this.Transaction = null;
    }

    public void CommitTransaction()
    {
        if (this.Transaction is null)
        {
            throw new TransactionException("There is no open transaction to be committed.");
        }

        if (this.Transaction.Connection is not null)
        {
            this.Transaction.Commit();
        }

        this.Transaction.Dispose();

        this.Transaction = null;
    }

    public async Task RollbackTransactionAsync()
    {
        if (this.Transaction is null)
        {
            throw new TransactionException("There is no open transaction to be rolled back.");
        }

        if (this.Transaction.Connection is not null)
        {
            await this.Transaction.RollbackAsync();
        }

        await this.Transaction.DisposeAsync();

        this.Transaction = null;
    }

    public void RollbackTransaction()
    {
        if (this.Transaction is null)
        {
            throw new TransactionException("There is no open transaction to be rolled back.");
        }

        if (this.Transaction.Connection is not null)
        {
            this.Transaction.Rollback();
        }

        this.Transaction.Dispose();

        this.Transaction = null;
    }

    public async ValueTask DisposeAsync()
    {
        await this.DisposeAsync(true);

        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsync(bool disposing)
    {
        if (!this.Disposed && disposing)
        {
            if (this.Transaction is not null)
            {
                await this.Transaction.RollbackAsync();

                await this.Transaction.DisposeAsync();
            }

            if (this.DbConnection.State != ConnectionState.Closed)
            {
                await this.DbConnection.CloseAsync();
            }

            await this.DbConnection.DisposeAsync();
        }

        this.Disposed = true;
    }

    public void Dispose()
    {
        this.Dispose(true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.Disposed && disposing)
        {
            if (this.Transaction is not null)
            {
                this.Transaction.Rollback();
                this.Transaction.Dispose();
            }

            if (this.DbConnection.State != ConnectionState.Closed)
            {
                this.DbConnection.Close();
            }

            this.DbConnection.Dispose();
        }

        this.Disposed = true;
    }
}
