using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RentIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Models.Movies
{
    /// <summary>
    /// A Dto that represents a Movie Entity
    /// </summary>
    public class AddMovieDto
    {
        /// <summary>
        /// The title of the movie
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The description of the movie
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// The date the movie was released
        /// </summary>
        [Required]
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// The rating of the movie provided by the MPAA
        /// </summary>
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public MovieRating Rating { get; set; }

        /// <summary>
        /// The genre of the movie
        /// </summary>
        [Required]
        public string Genre { get; set; }
    }
}
