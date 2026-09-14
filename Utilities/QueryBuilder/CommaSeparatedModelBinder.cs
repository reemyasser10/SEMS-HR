using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;

namespace Utilities.QueryBuilder
{
    public class CommaSeparatedModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // 1. Get the value provider (the query string data)
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            // 2. Get the actual string (e.g., "1,2,3")
            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            // 3. Determine the target type (List<int> or int[])
            var elementType = bindingContext.ModelType.GetGenericArguments().FirstOrDefault() ?? typeof(int);
            var converter = TypeDescriptor.GetConverter(elementType);

            // 4. Split and Convert
            try
            {
                var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(x => converter.ConvertFromString(x.Trim()))
                                  .ToList();

                // Create the final list of the correct type
                var typedList = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                var addMethod = typedList.GetType().GetMethod("Add");

                foreach (var v in values)
                {
                    addMethod.Invoke(typedList, new[] { v });
                }

                bindingContext.Result = ModelBindingResult.Success(typedList);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid format for numeric list.");
            }

            return Task.CompletedTask;
        }
    }
}
