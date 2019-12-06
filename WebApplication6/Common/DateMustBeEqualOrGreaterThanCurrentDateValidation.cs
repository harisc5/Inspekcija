using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebApplication6.Models;
using WebApplication6.Controllers;

namespace WebApplication6.Common
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class DateMustBeEqualOrGreaterThanCurrentDateValidation : ValidationAttribute, IClientValidatable
    {

        private const string DefaultErrorMessage = "Date selected {0} must be on or after today";

        public DateMustBeEqualOrGreaterThanCurrentDateValidation()
            : base(DefaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(DefaultErrorMessage, name);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateEntered = (DateTime)value;
            if (dateEntered < DateTime.Today)
            {
                var message = FormatErrorMessage(dateEntered.ToShortDateString());
                return new ValidationResult(message);
            }
            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientCustomDateValidationRule(FormatErrorMessage(metadata.DisplayName));
            yield return rule;
        }
    }

    public sealed class ModelClientCustomDateValidationRule : ModelClientValidationRule
    {

        public ModelClientCustomDateValidationRule(string errorMessage)
        {
            ErrorMessage = errorMessage;
            ValidationType = "datemustbeequalorgreaterthancurrentdate";
        }
    }
}