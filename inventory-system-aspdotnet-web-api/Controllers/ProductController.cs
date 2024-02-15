// ProductController.cs

using inventory_system_aspdotnet_web_api.Models;
using inventory_system_aspdotnet_web_api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace inventory_system_aspdotnet_web_api.Controllers
{
    [Authorize]
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _productRepository;

        public ProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/product
        [HttpGet]
        public ActionResult<IEnumerable<GetProduct>> Get()
        {
            try
            {
                var products = _productRepository.GetAllProducts();
                return Ok(new { data = products, success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/product
        [HttpPost]
        public ActionResult<IEnumerable<Product>> Post(Product product)
        {
            if (product == null) return BadRequest();

            try
            {
                var rowsAffected = _productRepository.AddOrUpdateProduct(null, product);
                if (rowsAffected == 1)
                    return Ok(new { message = "Product added successfully.", success = true });
                else return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/product/5
        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        public ActionResult<IEnumerable<Product>> Put(int id, Product product)
        {
            if (product == null || id == null) return BadRequest();

            try
            {
                var rowsAffected = _productRepository.AddOrUpdateProduct(id, product);
                if (rowsAffected > 0)
                    return Ok(new { message = "Product Updated successfully.", success = true });
                else return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/product/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if (id == null) return BadRequest();

            try
            {
                var rowsAffected = _productRepository.DeleteProduct(id);
                if (rowsAffected > 0)
                    return Ok(new { message = "Product Deleted successfully.", success = true });
                else return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
