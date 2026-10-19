using AwsCognito.Test.Domain.Exceptions;

namespace AwsCognito.Test.Domain.Samples
{
    /// <summary>Example value object. Replace or remove together with the Sample feature.</summary>
    public sealed record SampleName
    {
        public const int MaxLength = 100;

        public string Value { get; }

        public SampleName(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Trim().Length > MaxLength)
                throw new InvalidSampleNameException();

            Value = value.Trim();
        }

        public static SampleName FromTrusted(string value) => new(value);

        public override string ToString() => Value;
    }
}
