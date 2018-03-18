using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class CheckFuture: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext){
            DateTime Today = DateTime.Now;
            if((DateTime)value< Today)
            {
                return new ValidationResult("Date of Wedding can not be in the Past");
            }
            return ValidationResult.Success;
        }
    }
}