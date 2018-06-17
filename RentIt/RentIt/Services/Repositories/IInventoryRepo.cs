using System.Collections.Generic;
using RentIt.Data.Entities;

namespace RentIt.Services.Repositories
{
    /// <summary>
    /// Represents the interface for a repository for accessing and manipulating Inventory resources
    /// </summary>
    public interface IInventoryRepo
    {
        /// <summary>
        /// Checks in the Inventory Item with the given Id
        /// </summary>
        /// <param name="itemId">The Id of the Inventory Item</param>
        void CheckInItem(int itemId);

        /// <summary>
        /// Checks out the Inventory Item with the given Id
        /// </summary>
        /// <param name="itemId">The Id of the Inventory Item</param>
        void CheckOutItem(int itemId);

        /// <summary>
        /// Retreives all of the In-Stock Inventory Items for the given Movie Id
        /// </summary>
        /// <param name="movieId">The Id of the Movie associated with the Inventory Items</param>
        /// <returns>A collection of Inventory Items for the given Movie Id</returns>
        IEnumerable<InventoryItem> GetInStockItems(int movieId);
    }
}