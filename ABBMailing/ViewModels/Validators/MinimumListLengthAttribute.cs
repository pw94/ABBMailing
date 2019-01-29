using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ABBMailing.ViewModels
{
    public class MinimumListLengthAttribute : ValidationAttribute
    {
        private int _length;

        public MinimumListLengthAttribute(int length)
        {
            _length = length;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var addressVM = (AddressViewModel) validationContext.ObjectInstance;
            if (addressVM.Topics.Count < _length)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"List must have minimum {_length} elements.";
        }
    }
}