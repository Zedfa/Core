using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Core.Mvc.Attribute.Validation;

namespace Core.Mvc.Attribute
{
    public  static class RegisterAttribute
    {
      

        public static void SetRegisterAttribute()
        {

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(Range),
                typeof(RangeAttributeAdapter));

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(Required),
                typeof(RequiredAttributeAdapter));

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(StringLength),
                typeof(StringLengthAttributeAdapter));
        }



       
    }



    
}


