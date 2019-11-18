using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Countdown.Infrastructure
{
    public class DateTimeCustomValidation: ValidationAttribute
    {
        public String dateTimeString { get;}

        public DateTimeCustomValidation(string dateTimeString)
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (dateTimeString.Contains("/"))
                {
                    return new ValidationResult("Invalid Date");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            catch
            {
                return new ValidationResult("Invalid Date");
            }
        }
    }
}