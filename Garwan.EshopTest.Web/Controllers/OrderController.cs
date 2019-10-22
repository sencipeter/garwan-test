using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garwan.EshopTest.Business.Services.Interfaces;
using Garwan.EshopTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Garwan.EshopTest.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // POST: api/Order
        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dbResult = await _orderService.InsertOrder(order);
                    if (dbResult.Exception != null)
                        throw dbResult.Exception;
                    return Ok(dbResult);
                }
                else
                {
                    return BadRequest(string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage)));
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
    }
}
