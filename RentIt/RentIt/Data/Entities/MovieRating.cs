using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Data.Entities
{
    /// <summary>
    /// A rating for a movie defined by the MPAA to provide guidelines for what audiences the movie is appropriate for
    /// </summary>
    public enum MovieRating
    {
        /// <summary>
        /// General Audiences
        /// </summary>
        G,

        /// <summary>
        /// Parental Guidance Suggested
        /// </summary>
        PG,

        /// <summary>
        /// Parents Strongly Cautioned
        /// </summary>
        PG13,

        /// <summary>
        /// Restricted
        /// </summary>
        R,

        /// <summary>
        /// No one 17 and under admitted
        /// </summary>
        NC17
    }
}
