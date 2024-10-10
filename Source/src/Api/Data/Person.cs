using System.ComponentModel.DataAnnotations;

namespace OneStream.Api.Data
{
    public class Person
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
