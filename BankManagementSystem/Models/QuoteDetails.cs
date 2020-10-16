using System;
using System.ComponentModel.DataAnnotations;

namespace BankManagementSystem.Models
{
    public class QuoteDetails
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "CustomerId is Required")]
        public int CustomerId { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid Date Format")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid Date Format")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "ContributionAmount is Required")]
        public decimal ContributionAmount { get; set; }

        public decimal MaturityAmount { get; set; }
    }

}
