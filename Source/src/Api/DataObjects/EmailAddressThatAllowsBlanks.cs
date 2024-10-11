using System.ComponentModel.DataAnnotations;

namespace OneStream.Api.DataObjects
{
    public class EmailAddressThatAllowsBlanksAttribute() : ValidationAttribute(DefaultErrorMessage)
    {
        public const string DefaultErrorMessage = "{0} must be a valid email address";
        private readonly EmailAddressAttribute _validator = new();

        public override bool IsValid(object? value) => string.IsNullOrEmpty(value?.ToString()) || _validator.IsValid(value.ToString());
    }
}
