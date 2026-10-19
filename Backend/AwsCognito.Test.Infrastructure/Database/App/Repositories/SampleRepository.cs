using AwsCognito.Test.Application.Abstractions.Repositories;
using AwsCognito.Test.Domain.Samples;
using Microsoft.EntityFrameworkCore;

namespace AwsCognito.Test.Infrastructure.Database.App.Repositories
{
    public class SampleRepository : ISampleRepository
    {
        private readonly AppDbContext _context;

        public SampleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Sample?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Samples
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(SampleName name, CancellationToken cancellationToken = default)
        {
            return await _context.Samples.AnyAsync(x => x.Name == name, cancellationToken);
        }

        public async Task AddAsync(Sample sample, CancellationToken cancellationToken = default)
        {
            _context.Samples.Add(sample);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
