using BankManagementSystem.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankManagementSystem.Models
{
    public class CustomerDetails
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User name is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Address { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        public string PermanentAccountNumber { get; set; }

        [Required(ErrorMessage = "Date Of Birth is Required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date Format")]
        [CheckDateRange]
        public DateTime DateOfBirth { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Contact number should be of 10 digits")]
        public string ContactNumber { get; set; }

        public string AccountType { get; set; }
    }
}
