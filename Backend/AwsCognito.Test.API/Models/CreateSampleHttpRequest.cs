using System.ComponentModel.DataAnnotations;

namespace AwsCognito.Test.API.Models
{
    public sealed class CreateSampleHttpRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;
    }
}
