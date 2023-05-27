using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using RestaurantApi.Entities;
using RestaurantApi.Models;
using RestaurantApi.Services;

namespace RestaurantApi.Controllers
{
    [Route("api/restaurant")]
    [ApiController]                                         // <- ten atrybut sprawdza walidację przychodzących zapytań  |
    public class RestaurantController : ControllerBase      //    dla którego istnieje walidacja modelu    zastępuje to: V
    {
        public IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPut("{id}")]
        public ActionResult Modify([FromRoute] int id, [FromBody] ModifyRestaurantDto modDto)
        {
            //if (!ModelState.IsValid)         //  <--- walidacja modelu otrzymanego od usera                   <-------|
            //{
            //    return BadRequest(ModelState);
            //}

            _restaurantService.Modify(id, modDto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.Delete(id);

            return NoContent(); // No content nic nie zwraca ale jest z kategorii 200
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            //if (!ModelState.IsValid)                
            //{
            //    return BadRequest(ModelState);
            //}

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

            return Ok(restaurant);
        }
    }
}
