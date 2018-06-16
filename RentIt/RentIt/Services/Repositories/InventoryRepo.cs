using RentIt.Data;
using RentIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Services.Repositories
{
    /// <summary>
    /// Represents a repository for accessing and manipulating Inventory resources
    /// </summary>
    public class InventoryRepo
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the InventoryRepo class
        /// </summary>
        /// <param name="context">The AppDbContext injected via DI</param>
        public InventoryRepo(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retreives all of the In-Stock Inventory Items for the given Movie Id
        /// </summary>
        /// <param name="movieId">The Id of the Movie associated with the Inventory Items</param>
        /// <returns>A collection of Inventory Items for the given Movie Id</returns>
        public IEnumerable<InventoryItem> GetInStockItems(int movieId)
        {
            return _context.InventoryItems.Where(i => i.Movie.Id == movieId);
        }

        /// <summary>
        /// Checks in the Inventory Item with the given Id
        /// </summary>
        /// <param name="itemId">The Id of the Inventory Item</param>
        public void CheckInItem(int itemId)
        {
            UpdateCheckedInStatus(itemId, false);
        }

        /// <summary>
        /// Checks out the Inventory Item with the given Id
        /// </summary>
        /// <param name="itemId">The Id of the Inventory Item</param>
        public void CheckOutItem(int itemId)
        {
            UpdateCheckedInStatus(itemId, true);
        }

        private void UpdateCheckedInStatus(int itemId, bool isCheckedOut)
        {
            var item = _context.InventoryItems.SingleOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                throw new Exception($"Inventory Item ({itemId}) Not Found");
            }

            item.IsCheckedOut = isCheckedOut;

            _context.InventoryItems.Update(item);
            _context.SaveChanges();
        }
    }
}
