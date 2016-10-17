using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BangazonAPI.Data;
using BangazonAPI.Models;

namespace BangazonAPI.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private BangazonContext context;

        public CustomersController(BangazonContext ctx)
        {
            context = ctx;
        }

        // GET /Customers
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<object> customers = from customer in context.Customer select customer;

            if (customers == null)
            {
                return NotFound();
            }

            return Ok(customers);

        }

        // GET /Customers/5
        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Customer customer = context.Customer.Single(m => m.CustomerId == id);

                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(customer);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }


        }
        // POST /Customers
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Customer.Add(customer);
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // PUT Customer/5
       [HttpPut("{id}")]
       public IActionResult Put([FromBody] Customer customer, [FromRoute] int id)
       {
            //Check for bad model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Check for bad ID on Put object
            if(customer.CustomerId != id)
            {
                return BadRequest(customer);
            }

            //Update the customer to the context
            context.Customer.Update(customer);

            //EntityState.Modified

            //Try to save to DB, handle DB Update Issues
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            //Return successful creation
            return new StatusCodeResult(StatusCodes.Status201Created);
       }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {

            Customer customerToDelete = null;

            try
            {
                customerToDelete = context.Customer.Single(m => m.CustomerId == id);
            }
            catch (System.InvalidOperationException)
            {
                return NotFound();
            }

            context.Customer.Remove(customerToDelete);

            try
            {
                context.SaveChanges();
            }
                catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        
            return Ok(customerToDelete);
        }
        private bool CustomerExists(int id)
        {
            return context.Customer.Count(e => e.CustomerId == id) > 0;
        }
    }
}
