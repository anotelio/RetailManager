using System;
using System.Linq;
using RandomNameGeneratorLibrary;

namespace RetailManager.Data;

public sealed class RandomNameCreator
{
    private readonly IPersonNameGenerator personGenerator;

    public RandomNameCreator(IPersonNameGenerator personGenerator)
    {
        this.personGenerator = personGenerator;
    }

    private readonly static string[] appeals = new string[2]
    {
        "Mr.",
        "Mrs."
    };

    public string CustomerName => string.Format("{0} {1}",
        appeals.OrderBy(_ => Guid.NewGuid()).First(),
        GetRandomSurname());

    private string GetRandomSurname()
    {
        return this.personGenerator.GenerateRandomLastName();
    }
}
