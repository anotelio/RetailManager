namespace RetailManager.Contracts.Defaults;

public static class SqlQueryDefault
{
    public const string GetCustomerById = "select customer_id CustomerId, customer_name CustomerName from dbo.customers where customer_id = {0};";

    public const string AddCustomer = "insert into dbo.customers (customer_name) output inserted.customer_id values (N'{0}');";
}
