using AwsCognito.Test.Application.DTOs.Samples;

namespace AwsCognito.Test.Application.Abstractions.Services
{
    public interface ISampleService
    {
        Task<SampleDTO> GetById(Guid id, CancellationToken cancellationToken = default);

        Task<SampleDTO> Create(CreateSampleRequestDTO request, CancellationToken cancellationToken = default);
    }
}
