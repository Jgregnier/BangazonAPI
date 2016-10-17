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
    public class ProductsController : Controller
    {
        private BangazonContext context;

        public ProductsController(BangazonContext ctx)
        {
            context = ctx;
        }

        // GET Products
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<object> products = from product in context.Product select product;

            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);

        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetProduct")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Product product = context.Product.Single(m => m.ProductId == id);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }


        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Product.Add(product);
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.ProductId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetProduct", new { id = product.ProductId }, product);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody]Product product)
        {
            //Check for bad model on product put object
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Check for incorrect product ID on product object
            if (product.ProductId != id)
            {
                return BadRequest(product);
            }

            //Update the product in the context
            context.Product.Update(product);

            //Try to save the products to the DB, handle update exceptions
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            //Return succesfull update
            return new StatusCodeResult(StatusCodes.Status201Created);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            Product productToDelete = null;

            try
            {
                productToDelete = context.Product.Single(m => m.ProductId == id);
            }
            catch (System.InvalidOperationException)
            {
                return NotFound();
            }

            context.Product.Remove(productToDelete);

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            return Ok(productToDelete);
        }
        private bool ProductExists(int id)
        {
            return context.Product.Count(e => e.ProductId == id) > 0;
        }
    }
}
