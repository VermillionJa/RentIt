using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Extensions
{
    /// <summary>
    /// A collection of extension methods that can be used on the <see cref="char"/> class
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// Creates a new string with the character repeated the given number of times
        /// </summary>
        /// <param name="character">The character to repeat</param>
        /// <param name="count">The number of times to repeat the character</param>
        /// <returns>The string with the repeated character</returns>
        public static string Repeat(this char character, int count)
        {
            return new string(character, count);
        }
    }
}
