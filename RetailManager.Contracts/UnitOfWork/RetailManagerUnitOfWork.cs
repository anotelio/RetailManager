using System.Data.Common;
using RetailManager.Contracts.Interfaces;

namespace RetailManager.Contracts.UnitOfWork;

public sealed class RetailManagerUnitOfWork : UnitOfWork, IRetailManagerUnitOfWork
{
    public RetailManagerUnitOfWork(DbConnection dbConnection)
        : base(dbConnection)
    {
    }
}
