using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Models.Logging
{
    /// <summary>
    /// Represents a Collection of Log Details
    /// </summary>
    public class LogDetailCollection : IEnumerable<KeyValuePair<string, string>>
    {
        private readonly List<KeyValuePair<string, string>> _details = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Adds a new Log Detail to the Collection
        /// </summary>
        /// <param name="key">The Key</param>
        /// <param name="value">The Value</param>
        public void AddDetail(string key, string value)
        {
            _details.Add(new KeyValuePair<string, string>(key, value));
        }

        /// <summary>
        /// Gets the Enumerator for iterating over the Log Details
        /// </summary>
        /// <returns>The Log Details Enumerator</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _details.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
