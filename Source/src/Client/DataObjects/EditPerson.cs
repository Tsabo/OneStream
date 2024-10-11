using System.ComponentModel.DataAnnotations;

namespace OneStream.Client.DataObjects
{
    public class EditPerson
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [EmailAddressThatAllowsBlanks]
        [MaxLength(100)]
        public string? Email { get; set; }
    }
}
