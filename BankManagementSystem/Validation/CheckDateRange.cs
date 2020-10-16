using System;
using System.ComponentModel.DataAnnotations;

namespace BankManagementSystem.Validation
{
    public class CheckDateRange : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validationMessage = "";
            DateTime dateOfBirth = (DateTime)value;
            if (dateOfBirth == DateTime.Today)
            {
                validationMessage = "Date Of Birth cannot be today's date";
            }
            else if (dateOfBirth >= DateTime.Today)
            {
                validationMessage = "Date Of Birth cannot be future date";
            }
            else if (dateOfBirth >= DateTime.Today.AddYears(-16))
            {
                validationMessage = "Age of customer should be atleast 16 years";
            }
            if (!string.IsNullOrEmpty(validationMessage))
                return new ValidationResult(ErrorMessage ?? validationMessage);
            return ValidationResult.Success;
        }
    }
}
