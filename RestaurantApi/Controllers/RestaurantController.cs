using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using RestaurantApi.Entities;
using RestaurantApi.Models;
using RestaurantApi.Services;

namespace RestaurantApi.Controllers
{
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        public IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPut("{id}")]
        public ActionResult Modify([FromRoute] int id, [FromBody] ModifyRestaurantDto modDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isModified = _restaurantService.Modify(id, modDto);

            if (isModified) return Ok(); 
            return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var isDeleted = _restaurantService.Delete(id);

            if (isDeleted) return NoContent(); // No content nic nie zwraca ale jest z kategorii 200
            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            if (!ModelState.IsValid)                //  <--- walidacja modeluotrzymanego od usera
            {
                return BadRequest(ModelState);
            }

            var id = _restaurantService.Create(dto);
            return Created($"/api/restaurant/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var restaurantsDtos = _restaurantService.GetAll();
            return Ok(restaurantsDtos);
        }

        [HttpGet("{RestaurantId}")]
        public ActionResult<RestaurantDto> Get([FromRoute] int RestaurantId)
        {
            var restaurant = _restaurantService.GetById(RestaurantId);

            if (restaurant == null) return NotFound();

            return Ok(restaurant);
        }
    }
}
