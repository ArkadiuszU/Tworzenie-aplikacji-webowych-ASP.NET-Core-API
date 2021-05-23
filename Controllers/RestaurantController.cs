using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            var restaurantsDtos = _restaurantService.GetAll();
            return Ok(restaurantsDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> GetById([FromRoute] int id )
        {
            var restaurant = _restaurantService.GetById(id);

            return Ok(restaurant);

        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {

            int restaurantId = _restaurantService.Create(dto);

            return Created($"/api/restaurant/{restaurantId}", null);
         
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
             _restaurantService.Delete(id);
        
            return NoContent();
           
        }

        [HttpPut("{id}")]
        public ActionResult Edit([FromRoute] int id, [FromBody] EditRestaurantDto dto)
        {
            // atrybut [ApiControler] ogarnia sprawę 
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _restaurantService.Edit(id, dto);

            return NoContent();
        }

    }
}
