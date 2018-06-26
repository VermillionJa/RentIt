using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Models.ObjectResults
{
    /// <summary>
    /// Represents an Conflict (HTTP Status 409) Response
    /// </summary>
    public class ConflictObjectResult : ObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the ConflictObjectResult
        /// </summary>
        /// <param name="message">A message for the client on what is conflicting</param>
        public ConflictObjectResult(string message) : base(message)
        {
            StatusCode = StatusCodes.Status409Conflict;
        }
    }
}
