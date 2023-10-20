using System.ComponentModel.DataAnnotations;

namespace Blog_API.CustomValidations
{
    public class CustomValidatorsForYear : ValidationAttribute
    {
        public int MinimumYear { get; set; }
        public string DefaultErroMessage { get; set } = "Some Custom Error Message";
        public CustomValidatorsForYear()
        {
            
        }
        public CustomValidatorsForYear(int minimumYear)
        {
            MinimumYear = minimumYear;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value != null)
            {
                DateTime date = (DateTime)value;
                if(date.Year < MinimumYear)
                {
                    // Minimum Year Will get index 0 at run Time when if passed By String like this => {0}
                    return new ValidationResult(string.Format(ErrorMessage ?? DefaultErroMessage, MinimumYear));
                }

                return ValidationResult.Success;

            }
            return null;
           
        }
    }
}
