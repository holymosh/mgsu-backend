using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MongoDB.Bson;

namespace MGSUCore.Controllers.Extentions
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ObjectIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(MongoDB.Bson.ObjectId.TryParse(value.ToString(), out var idParsed))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("'Id' parameter is ivalid ObjectId");
        }

        public override bool IsValid(object value)
        {
            if(MongoDB.Bson.ObjectId.TryParse(value.ToString(), out var idParsed))
            {
                return true;
            }
            return false;
        }
    }
}