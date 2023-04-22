using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LanguageBuddy.Models.Validatos
{
    public class LanguageReady : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (typeof(Language).IsInstanceOfType(validationContext.ObjectInstance))
            {
                /*
                Language gol = (Language)validationContext.ObjectInstance;
                ApplicationDbContext bd = new ApplicationDbContext();

                List<Language> goals = bd.Languages.Where(
                        x => x.Name == gol.Name
                    ).ToList();

                if ( goals != null )
                {
                    if ( goals.Count != 0 )
                    {
                        return new ValidationResult("This is already in the db");
                    }
                }
                */
                return ValidationResult.Success;
            }

            return ValidationResult.Success;
        }

    }
}