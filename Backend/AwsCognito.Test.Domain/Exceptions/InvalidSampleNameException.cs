namespace AwsCognito.Test.Domain.Exceptions
{
    public class InvalidSampleNameException : DomainException
    {
        public InvalidSampleNameException()
            : base("sample.invalid_name", "The sample name is invalid.") { }
    }
}
