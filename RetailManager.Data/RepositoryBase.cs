using RetailManager.Contracts.Interfaces;

namespace RetailManager.Data;

public abstract class RepositoryBase
{
    public readonly IUnitOfWork unitOfWork;

    protected RepositoryBase(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
}
