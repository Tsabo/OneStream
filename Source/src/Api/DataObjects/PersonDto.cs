using System.ComponentModel.DataAnnotations;

namespace OneStream.Api.DataObjects
{
    public class PersonDto
    {
        public Guid Id { get; set; } = Guid.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
