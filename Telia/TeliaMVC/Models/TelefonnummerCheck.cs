using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TeliaMVC.Models
{
    public class TelefonnummerCheck : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int valueInteger;
                if (int.TryParse(value.ToString(), out valueInteger))
                {
                    if (value.ToString().Length<= 13) // funkcija koja treba da proveri sledece: 
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(string.Concat(validationContext.DisplayName, " Its not valid! "));
                    }
                }
                else
                {
                    return new ValidationResult(string.Concat(validationContext.DisplayName, " must be number not letter!"));
                }
            }
            return ValidationResult.Success;
        }
        /*private bool Check(int number)
        {
            int firstDigit = (int)(number / Math.Pow(10, (int)Math.Floor(Math.Log10(number))));
            if (firstDigit == 4)
            {
                return true;
            }
            else if (firstDigit == 9)
            {
                return true;
            }
            else { return false; }
        }*/
    }
}