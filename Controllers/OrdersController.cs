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
    public class OrdersController : Controller
    {
        private BangazonContext context;

        public OrdersController(BangazonContext ctx)
        {
            context = ctx;
        }

        // GET Orders
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<object> orders = from order in context.Order select order;

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);

        }

        // GET orders/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Order order = context.Order.Single(m => m.CustomerId == id);

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }


        }
        // POST /orders
        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Order.Add(order);
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.CustomerId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetOrder", new { id = order.OrderId }, order);
        }

        // PUT orders/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody]Order order)
        {
            //Check for bad model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Check for bad OrderId on order object
            if (order.OrderId != id)
            {
                return BadRequest(order);
            }

            //Update Order in the context
            context.Order.Update(order);

            //Try to save to DB, handle DB update issues
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

        // DELETE orders/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            Order orderToDelete = null;

            try
            {
                orderToDelete = context.Order.Single(m => m.OrderId == id);
            }
            catch (System.InvalidOperationException)
            {
                return NotFound();
            }

            context.Order.Remove(orderToDelete);

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            return Ok(orderToDelete);
        }
        private bool OrderExists(int id)
        {
            return context.Order.Count(e => e.OrderId == id) > 0;
        }
    }
}