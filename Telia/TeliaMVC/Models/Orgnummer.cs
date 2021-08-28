using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeliaMVC.Models
{
    public class Orgnummer : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int valueInteger;
                if (int.TryParse(value.ToString(), out valueInteger))
                {
                    if (valueInteger.ToString().Length == 9) // funkcija koja treba da proveri sledece: 
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(string.Concat(validationContext.DisplayName, " Ikke gyldig,trenger 9 tall"));
                    }
                }
                else
                {
                    return new ValidationResult(string.Concat(validationContext.DisplayName, " Må være tall"));
                }
            }
            return ValidationResult.Success;
        }
    }
}