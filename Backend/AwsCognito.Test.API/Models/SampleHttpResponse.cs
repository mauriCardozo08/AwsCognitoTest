namespace AwsCognito.Test.API.Models
{
    public sealed class SampleHttpResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime CreatedAtUtc { get; set; }
    }
}
