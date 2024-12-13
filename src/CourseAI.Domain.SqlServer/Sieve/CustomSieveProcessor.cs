using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace CourseAI.Domain.Sieve;

public class CustomSieveProcessor(
    IEnumerable<ISieveConfiguration> sieveConfigurations,
    IOptions<SieveOptions> options,
    ISieveCustomSortMethods customSortMethods,
    ISieveCustomFilterMethods customFilterMethods)
    : SieveProcessor(options, customSortMethods, customFilterMethods)
{
    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        foreach (var configuration in sieveConfigurations)
        {
            configuration.Configure(mapper);
        }

        return mapper;
    }
}