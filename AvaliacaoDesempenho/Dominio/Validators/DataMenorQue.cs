using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AvaliacaoDesempenho.Dominio.Validators
{
    public class DataMenorQue : ValidationAttribute
    {
        public DateTime Ate { get; set; }
        
        public DataMenorQue(string ate)
            :base("A {0} é maior que a data final.")
        {
            if (!string.IsNullOrEmpty(ate.ToString()))
            {
                Ate = Convert.ToDateTime(ate);
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if(!string.IsNullOrEmpty(value.ToString()))
                {
                    DateTime de = Convert.ToDateTime(value.ToString());
                    if (de > Ate)
                    {
                        var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                        return new ValidationResult(errorMessage);
                    }
                }
                
            }

            return ValidationResult.Success;
        }
    }
}