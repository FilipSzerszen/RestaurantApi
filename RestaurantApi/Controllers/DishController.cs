using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using RestaurantApi.Models;
using RestaurantApi.Services;

namespace RestaurantApi.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]

    public class DishController : ControllerBase
    {
        public readonly IDishService _dishService;

        public DishController (IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int restaurantId)
        {
            _dishService.DeleteAll(restaurantId);

            return NoContent();
        }

        [HttpDelete("{dishId}")]
        public ActionResult Delete([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            _dishService.Delete(restaurantId, dishId);

            return NoContent();
        }

        [HttpGet]
        public ActionResult<List<DishDto>> Get([FromRoute] int restaurantId)
        {
            var menu = _dishService.GetAll(restaurantId);

            return Ok(menu);
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            DishDto dish = _dishService.GetById(restaurantId, dishId);

            return Ok(dish);
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int restaurantId, [FromBody]CreateDishDto dto)
        {
            var newDishId = _dishService.Create(restaurantId, dto);
            
            return Created($"/api/restaurant/{restaurantId}/dish/{newDishId}", null);
        }
    }
}
