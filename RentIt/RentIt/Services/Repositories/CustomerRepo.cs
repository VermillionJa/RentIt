using RentIt.Data;
using RentIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Services.Repositories
{
    /// <summary>
    /// Represents a repository for accessing and manipulating Customer resources
    /// </summary>
    public class CustomerRepo
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the CustomersRepo class
        /// </summary>
        /// <param name="context">The AppDbContext injected via DI</param>
        public CustomerRepo(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all of the Customers
        /// </summary>
        /// <returns>A collection of Customers</returns>
        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers;
        }

        /// <summary>
        /// Get the Customer with the given Id
        /// </summary>
        /// <param name="id">The Id of the Customer to get</param>
        /// <returns>The Customer with the given Id</returns>
        public Customer GetById(int id)
        {
            return _context.Customers.SingleOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Adds the given Customer to the Repository
        /// </summary>
        /// <param name="customer">The Customer to add</param>
        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates a Customer
        /// </summary>
        /// <param name="customer">The updated Customer data</param>
        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        /// <summary>
        /// Removes the Customer with the given Id
        /// </summary>
        /// <param name="id">The Id of the Customer to remove</param>
        public void Remove(int id)
        {
            Remove(GetById(id));
        }

        /// <summary>
        /// Removes the given Customer
        /// </summary>
        /// <param name="customer">The Customer to remove</param>
        public void Remove(Customer customer)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }
    }
}
