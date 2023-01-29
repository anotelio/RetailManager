using System.Data.Common;
using RetailManager.Contracts.Interfaces;

namespace RetailManager.Contracts.UnitOfWork;

public sealed class UserUnitOfWork : UnitOfWork, IUserUnitOfWork
{
    public UserUnitOfWork(DbConnection dbConnection)
        : base(dbConnection)
    {
    }
}
