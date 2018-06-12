using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Data.Entities
{
    /// <summary>
    /// Represents a movie
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// The Id of the movie
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The title of the movie
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of the movie
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The date the movie was released
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// The rating of the movie provided by the MPAA
        /// </summary>
        public MovieRating Rating { get; set; }

        /// <summary>
        /// The genre of the movie
        /// </summary>
        public MovieGenre Genre { get; set; }
    }
}
