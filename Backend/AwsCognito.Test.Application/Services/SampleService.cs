using AwsCognito.Test.Application.Abstractions.Repositories;
using AwsCognito.Test.Application.Abstractions.Services;
using AwsCognito.Test.Application.DTOs.Samples;
using AwsCognito.Test.Application.Exceptions;
using AwsCognito.Test.Domain.Samples;

namespace AwsCognito.Test.Application.Services
{
    public class SampleService : ISampleService
    {
        private readonly ISampleRepository _sampleRepository;

        public SampleService(ISampleRepository sampleRepository)
        {
            _sampleRepository = sampleRepository;
        }

        public async Task<SampleDTO> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var sample = await _sampleRepository.GetByIdAsync(id, cancellationToken);

            if (sample is null)
                throw new SampleNotFoundException(Resources.ErrorMessages.SampleNotFound);

            return ToDto(sample);
        }

        public async Task<SampleDTO> Create(CreateSampleRequestDTO request, CancellationToken cancellationToken = default)
        {
            var name = new SampleName(request.Name);

            if (await _sampleRepository.ExistsByNameAsync(name, cancellationToken))
                throw new SampleAlreadyExistsException(Resources.ErrorMessages.SampleAlreadyExists);

            var sample = new Sample
            {
                Id = Guid.NewGuid(),
                Name = name,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _sampleRepository.AddAsync(sample, cancellationToken);

            return ToDto(sample);
        }

        private static SampleDTO ToDto(Sample sample) =>
            new(sample.Id, sample.Name.Value, sample.CreatedAtUtc);
    }
}
