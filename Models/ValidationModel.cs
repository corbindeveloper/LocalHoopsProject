#pragma warning disable CS8765
#pragma warning disable CS8603
using System.ComponentModel.DataAnnotations;
namespace CSharpExam.Models;

public class FutureDate: ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {   
        DateTime current = DateTime.Now, given = (DateTime)value;

        if (given < current)
        {
            return new ValidationResult("must be in the future");

        }
        return ValidationResult.Success;
    }
}