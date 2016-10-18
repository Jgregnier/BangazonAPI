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
    public class LineItemsController : Controller
    {
        private BangazonContext context;

        public LineItemsController(BangazonContext ctx)
        {
            context = ctx;
        }

        // GET /LineItems
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<object> lineItems = from lineItem in context.LineItem select lineItem;

            if (lineItems == null)
            {
                return NotFound();
            }

            return Ok(lineItems);

        }

        // GET /LineItem/5
        [HttpGet("{id}", Name = "GetLineItem")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                LineItem lineItem = context.LineItem.Single(m => m.LineItemId == id);

                if (lineItem == null)
                {
                    return NotFound();
                }

                return Ok(lineItem);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }


        }
        // POST /LineItem
        [HttpPost]
        public IActionResult Post([FromBody] LineItem lineItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.LineItem.Add(lineItem);

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            return CreatedAtRoute(new { id = lineItem.LineItemId }, lineItem);
        }

        // PUT Customer/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] LineItem lineItem, [FromRoute] int id)
        {
            //Check for bad model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Check for bad ID on Put object
            if (lineItem.LineItemId != id)
            {
                return BadRequest(lineItem);
            }

            //Update the customer to the context
            try
            {
                context.LineItem.Update(lineItem);
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

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
            return new StatusCodeResult(StatusCodes.Status202Accepted);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {

            LineItem lineItemToDelete = null;

            try
            {
                lineItemToDelete = context.LineItem.Single(m => m.LineItemId == id);
            }
            catch (System.InvalidOperationException)
            {
                return NotFound();
            }

            context.LineItem.Remove(lineItemToDelete);

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }
        private bool LineItemExists(int id)
        {
            return context.LineItem.Count(e => e.LineItemId == id) > 0;
        }
    }
}

