using Blog_API.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Blog_API.CustomValidations
{
    public class CustomCompareValidation : ValidationAttribute
    {

        //This Example Compare toDate and fromDate for example incoming DTO

        public string OtherPropertyName { get; set; }
        public CustomCompareValidation(string otherPropertyName)
        {
            OtherPropertyName = otherPropertyName;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)

        {
            if (value != null)
            {
                DateTime toDate = Convert.ToDateTime(value);
                //We want refrence of otherProperty of Dto with given name Duo the reflection => validationContext.ObjectType this line gives us DTO
                PropertyInfo? otherProperty = validationContext.ObjectType.GetProperty(OtherPropertyName);
                //In Above Code we have refrence of OtherProperty Now We Must find value of this due the reflection //validationContext.ObjectInstance this gives us Instance of Dto
                object fromDateObject = otherProperty.GetValue(validationContext.ObjectInstance);

                if(OtherPropertyName != null)
                {

              
                DateTime fromDate = Convert.ToDateTime(fromDateObject);

                //Bussines Logic
                //validationContext.MemberName gives us Name of property wich Attribute aplied to it
                    if (fromDate < toDate)
                    {
                    return new ValidationResult(ErrorMessage, new string[] { OtherPropertyName, validationContext.MemberName! });
                    }
                    return ValidationResult.Success;
                }
                return null;
            }
            return null;
        }
    }
}

public class ExampleDto
{
    DateTime FromDate { get; set; }
    [CustomCompareValidation("FromDate" , ErrorMessage ="Custom ErrorMessage")]
    DateTime ToDate { get; set; }
}