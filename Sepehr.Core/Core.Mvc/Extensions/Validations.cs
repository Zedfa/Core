using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Core.Mvc.Extensions
{
  
    public class Required : ValidationBase
    {
        public override Dictionary<string, string> CreateRelatedValidation()
        {
            var _constantService = Cmn.AppBase.DependencyInjectionManager.Resolve<Service.IConstantService>();
            var msg = string.Empty;
            _constantService.TryGetValue<string>("FillTheField", out msg);
            return new Dictionary<string, string> { { "data-val-required", msg/*Core.Resources.Messages.FillTheField */} };
        }

    }

    public class PasswordChecker : ValidationBase
    {
        public override Dictionary<string, string> CreateRelatedValidation()
        {
            var _constantService = Cmn.AppBase.DependencyInjectionManager.Resolve<Service.IConstantService>();

            var passwordMorethan5Msg = string.Empty;
            _constantService.TryGetValue<string>("PasswordMustBeMoreThan5Character", out passwordMorethan5Msg);

            var passwordContainsDigitsAndCharsMsg = string.Empty;
            _constantService.TryGetValue<string>("PasswordMustBeContainCharactersAndDigits", out passwordContainsDigitsAndCharsMsg);

            return new Dictionary<string, string> { 

                    {"data-val-length",passwordMorethan5Msg /*Core.Resources.Messages.PasswordMustBeMoreThan5Character*/},

                    {"data-val-length-min", "5"},

                    {"data-val-regex", passwordContainsDigitsAndCharsMsg/*Core.Resources.Messages.PasswordMustBeContainCharactersAndDigits*/},

                    {"data-val-regex-pattern", @"\D+\d+|\d+\D+"}
           };
        }
    }

    public class StrengthChecker : ValidationAttribute
    {
        private int MinLenght { get; set; }

        private string Pattern { get; set; }

        public StrengthChecker(int minLenght = 5 , string pattern=@"\D+\d+|\d+\D+" )
        {
            this.MinLenght = minLenght;

            this.Pattern = pattern;

            var constantService = Cmn.AppBase.DependencyInjectionManager.Resolve<Service.IConstantService>();
            var passwordIsNotValidMsg = string.Empty;
            constantService.TryGetValue<string>("PasswordIsNotValid", out passwordIsNotValidMsg);

            this.ErrorMessage = passwordIsNotValidMsg;

        }

        public override bool IsValid(object value)
        {
            string str = (string)value;

            if (str.Length > this.MinLenght)
            {
                Regex rg = new Regex(this.Pattern);

                return rg.IsMatch(str);
            }

            return false;
        }
    }


  
    public class Range : ValidationBase
    {
        private int Min { get; set; }
        private int Max { get; set; }
        public Range(int min, int max)
        {
            Max = max;
            Min = min;
        }

      
        public override Dictionary<string, string> CreateRelatedValidation()
        {
            throw new NotImplementedException();

        }
    }

  
    public abstract class ValidationBase
    {
        public abstract Dictionary<string, string> CreateRelatedValidation();
    }

}
