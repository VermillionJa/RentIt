using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Models.ObjectResults
{
    /// <summary>
    /// Represents an Unprocessable Entity (HTTP Status 422) Response
    /// </summary>
    public class UnprocessableEntityObjectResult : ObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the UnprocessableEntityObjectResult configured using the given ModelStateDictionary
        /// </summary>
        /// <param name="modelState">The ModelStateDictionary containing the errors to return to the client</param>
        public UnprocessableEntityObjectResult(ModelStateDictionary modelState) : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
