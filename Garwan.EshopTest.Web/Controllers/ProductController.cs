using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garwan.EshopTest.Business.Requests;
using Garwan.EshopTest.Business.Services;
using Garwan.EshopTest.Business.Services.Interfaces;
using Garwan.EshopTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Garwan.EshopTest.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("detail/{id}")]        
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {                
                var dbResult = await _productService.Get(id);
                if (dbResult.Result == null)
                    return NotFound(id);
                return Ok(dbResult);
            }
            catch ( Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [Route("list")]
        [HttpGet]        
        public async Task<IActionResult> GetList(int? page=null, int? pageSize = null, string orderBy=null,string orderDirection = null)
        {  
            try            
            {
                
                var request = new ProductRequest 
                {
                    Page = page ?? 1,
                    PageSize = pageSize ?? 10,
                    OrderBy = orderBy ?? "Id",
                    OrderDirection = orderDirection ?? "desc"
                }; 
                var dbResult = await _productService.Get(request);
                if (dbResult.Exception != null)
                    throw dbResult.Exception;
                return Ok(dbResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
       
        public async Task<IActionResult> Insert([FromBody] Product product)
        {
            try
            {
                var dbResult = await _productService.InsertUpdate(product);
                return Ok(dbResult.GetResultOrThrowException());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
    }
}