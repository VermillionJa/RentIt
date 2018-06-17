using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Data.Entities
{
    /// <summary>
    /// Represents a physical copy of a movie that has been added to the store's inventory
    /// </summary>
    public class InventoryItem
    {
        /// <summary>
        /// The Id of the inventory item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The movie on the disk
        /// </summary>
        public Movie Movie { get; set; }

        /// <summary>
        /// The storage format of the disk
        /// </summary>
        public MovieStorageFormat StorageFormat { get; set; }

        /// <summary>
        /// Is this copy of the movie checked out of the inventory?
        /// </summary>
        public bool IsCheckedOut { get; set; }
    }
}
