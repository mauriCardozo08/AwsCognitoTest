using AwsCognito.Test.Domain.Samples;

namespace AwsCognito.Test.Application.Abstractions.Repositories
{
    public interface ISampleRepository
    {
        Task<Sample?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> ExistsByNameAsync(SampleName name, CancellationToken cancellationToken = default);

        Task AddAsync(Sample sample, CancellationToken cancellationToken = default);
    }
}
