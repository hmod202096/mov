using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.TagHelpers
{
    public class MyCustomValidationAttribute : ValidationAttribute
    {
        public MyCustomValidationAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                //string gmail = value.ToString();
                //if (gmail.Contains(Text))
                //{
                //    return ValidationResult.Success;
                //}

                string[] strings = value.ToString().Split('@');

                if (strings[1].ToUpper().Contains(Text.ToUpper()))
                {
                    return ValidationResult.Success;
                }

            }

            return new ValidationResult(ErrorMessage ?? "يجب أن يكون Gmail.com");
        }
    }
}
