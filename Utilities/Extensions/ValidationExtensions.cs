

using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Utilities.Extensions
{
    public static class ValidationExtensions
    {
        /// <summary>
        /// Adds FluentValidation errors to ASP.NET ModelState
        /// </summary>
        /// <param name="modelState">The ModelState dictionary</param>
        /// <param name="validationResult">The FluentValidation result</param>
        public static void AddValidationErrors(this ModelStateDictionary modelState,
                                                 ValidationResult validationResult)
        {
            if (validationResult == null || validationResult.IsValid)
                return;

            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }

        /// <summary>
        /// Gets all error messages from ValidationResult as a single string
        /// </summary>
        /// <param name="validationResult">The FluentValidation result</param>
        /// <param name="separator">Separator between error messages (default: line break)</param>
        /// <returns>Concatenated error messages</returns>
        public static string GetErrorMessages(this ValidationResult validationResult,
                                               string separator = "\n")
        {
            if (validationResult == null || validationResult.IsValid)
                return string.Empty;

            return string.Join(separator, validationResult.Errors.Select(e => e.ErrorMessage));
        }

     
    }

}
