using System.Threading.Tasks;
using Dapper;
using RetailManager.Contracts.DataSets;
using RetailManager.Contracts.Defaults;
using RetailManager.Contracts.Interfaces;

namespace RetailManager.Data;

public class CustomerRepository : RepositoryBase
{
    public CustomerRepository(IRetailManagerUnitOfWork retailManagerUnitOfWork)
        : base(retailManagerUnitOfWork)
    {
    }

    public Task<CustomerDataSet> GetById(long customerId)
    {
        string sqlQuery =
            string.Format(SqlQueryDefault.GetCustomerById, customerId);

        return this.unitOfWork.DbConnection
            .QuerySingleOrDefaultAsync<CustomerDataSet>(sqlQuery);
    }

    public Task<long> Add(string customerName)
    {
        string sqlQuery =
            string.Format(SqlQueryDefault.AddCustomer, customerName);

        return this.unitOfWork.DbConnection
            .QuerySingleAsync<long>(sqlQuery);
    }
}
