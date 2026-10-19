namespace AwsCognito.Test.Domain.Samples
{
    /// <summary>Example aggregate. Replace or remove together with the Sample feature.</summary>
    public class Sample
    {
        public Guid Id { get; set; }
        public SampleName Name { get; set; } = default!;
        public DateTime CreatedAtUtc { get; set; }
    }
}
