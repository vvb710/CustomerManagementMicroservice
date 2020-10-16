using BankManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankManagementSystem.Extensions
{
    public static class MaturityCalculation
    {
        public static decimal CalculateMaturityAmount(QuoteDetails quoteDetails)
        {
            decimal maturityAmount = quoteDetails.ContributionAmount + quoteDetails.ContributionAmount * GetRateOfInterest(quoteDetails.StartDate, quoteDetails.EndDate);
            return maturityAmount;
        }

        private static decimal GetRateOfInterest(DateTime startDate, DateTime endDate)
        {
            var noOfDays = (endDate - startDate).TotalDays;
            if (noOfDays <= 30) return 0.5m;
            else if (noOfDays > 30 && noOfDays <= 90) return 1.5m;
            else if (noOfDays > 90 && noOfDays <= 120) return 2m;
            else if (noOfDays > 120) return 5m;
            return 0m;
        }
    }
}
