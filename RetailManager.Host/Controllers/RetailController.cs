using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RetailManager.Contracts.Responses;
using RetailManager.Data;

namespace RetailManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RetailController : ControllerBase
{
    private readonly CustomerRepository customerRepository;

    private readonly RandomNameCreator randomNameCreator;

    public RetailController(CustomerRepository customerRepository,
        RandomNameCreator randomNameCreator)
    {
        this.customerRepository = customerRepository;
        this.randomNameCreator = randomNameCreator;
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<CustomerResponse>> CustomerGet(long customerId)
    {
        (long CustomerId, string CustomerName) =
            await this.customerRepository.GetById(customerId);

        CustomerResponse customer = new(CustomerId, CustomerName);

        return Ok(customer);
    }

    [HttpGet("addCustomer")]
    public async Task<ActionResult<long>> CustomerAdd()
    {
        return Ok(await this.customerRepository
            .Add(this.randomNameCreator.CustomerName));
    }
}
