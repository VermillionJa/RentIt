using Microsoft.AspNetCore.Mvc.ModelBinding;
using RentIt.Models.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentIt.Extensions
{
    /// <summary>
    /// A collection of extension methods that can be used on the <see cref="LogDetailCollection"/>
    /// </summary>
    public static class LogDetailCollectionExtensions
    {
        private const char Tab = '\t';

        /// <summary>
        /// Adds a Log Detail to the Log Detail Collection
        /// </summary>
        /// <param name="detailCollection">The Log Detail Collection to add to</param>
        /// <param name="key">The Key for the Detail</param>
        /// <param name="value">The Value of the Detail</param>
        /// <returns>The updated Log Detail Collection</returns>
        public static LogDetailCollection Add(this LogDetailCollection detailCollection, string key, string value)
        {
            detailCollection.AddDetail(key, value);

            return detailCollection;
        }

        /// <summary>
        /// Adds a Log Detail to the Log Detail Collection
        /// </summary>
        /// <param name="detailCollection">The Log Detail Collection to add to</param>
        /// <param name="key">The Key for the Detail</param>
        /// <param name="value">The Value of the Detail</param>
        /// <returns>The updated Log Detail Collection</returns>
        public static LogDetailCollection Add(this LogDetailCollection detailCollection, string key, object value)
        {
            return detailCollection.Add(key, value.ToString());
        }

        /// <summary>
        /// Adds all of the errors in the given ModelState to the Log Detail Collection
        /// </summary>
        /// <param name="detailCollection">The Log Detail Collection to add to</param>
        /// <param name="modelState">The Model State containing the errors to add</param>
        /// <returns>The updated Log Detail Collection</returns>
        public static LogDetailCollection AddModelState(this LogDetailCollection detailCollection, ModelStateDictionary modelState)
        {
            var value = new StringBuilder();

            value.AppendLine();

            foreach (var entry in modelState)
            {
                value.AppendLine($"{GetTabs(1)}{entry.Key}");

                foreach (var error in entry.Value.Errors)
                {
                    value.AppendLine($"{GetTabs(2)}{error.ErrorMessage}");
                }
            }

            return detailCollection.Add("ModelState", value.ToString());
        }

        private static string GetTabs(int count)
        {
            return new string(Tab, count);
        }
    }
}
