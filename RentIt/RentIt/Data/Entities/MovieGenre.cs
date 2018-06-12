using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Data.Entities
{
    /// <summary>
    /// Represents a genre of movies
    /// </summary>
    public class MovieGenre
    {
        /// <summary>
        /// The Id of the genre
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the genre
        /// </summary>
        public string Name { get; set; }
    }
}
