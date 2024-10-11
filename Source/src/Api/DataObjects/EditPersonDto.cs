using System.ComponentModel.DataAnnotations;

namespace OneStream.Api.DataObjects
{
    public class EditPersonDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [EmailAddressThatAllowsBlanks]
        [MaxLength(100)]
        public string? Email { get; set; }
    }
}
