using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RestaurantApi.Entities;
using RestaurantApi.Models;

namespace RestaurantApi.Controllers
{
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;                                           // <- tu dodajemy dodatkowe pole -|
        public RestaurantController(RestaurantDbContext dbContext, IMapper mapper)  // <-wstrzykujemy w konstruktor  -|
        {                                                                           //                 mapper         |
            _dbContext = dbContext;                                                 //                                |
            _mapper = mapper;                                                       // <- tu przypisujemy mapper     -|
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            if(!ModelState.IsValid)     // sprawdzenie poprawności przesłanych danych
            {
                return BadRequest(ModelState);
            }
            var restaurant = _mapper.Map<Restaurant>(dto); //mapowanie obiektu dto na obiekt Restaurant
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return Created($"/api/restaurant/{restaurant.Id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var restaurants = _dbContext
                .Restaurants
                .ToList();

            return Ok(restaurants);
        }

        [HttpGet("{RestaurantId}")]                                 // proste zapytanie
        public ActionResult<Restaurant> GetOne([FromRoute] int RestaurantId)
        {
            var restaurant = _dbContext
                .Restaurants
                //.Select(r=>new {r.Id, r.Name, r.Description})       //selektor danych
                .FirstOrDefault(i => i.Id == RestaurantId);

            if (restaurant == null) return NotFound();
            return Ok(restaurant);
        }

        [HttpGet("getdto")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAllForDto()
        {
            var restaurants = _dbContext
                .Restaurants                                // <- zaincludować tabele   ------------------|
                .Include(r=>r.Address)
                .Include(d=>d.Dishes)
                .ToList();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            return Ok(restaurantsDto);
        }

        [HttpGet("getdto/{RestaurantId}")]              // zapytanie z DTO (data transfer object)
        public ActionResult<RestaurantDto> GetDto([FromRoute] int RestaurantId)
        {
            //var restaurant = _dbContext
            //.Restaurants;
            //.Select(r => new                                // pierwsza opcja ale upierdliwa
            //{
            //    Name = r.Name,
            //    Category = r.Category,
            //    City = r.Address.City,
            //    Street = r.Address.Street,                                                                A
            //    PostalCode = r.Address.PostalCode                                                         |
            //});                                                                                           |
            var restaurant = _dbContext                     // <- opcja druga za pomocą mappera, dodajemy --|
                .Restaurants                                //     i nie zapomnieć o serwisie mappera w klasie startup!!!
                .FirstOrDefault(i => i.Id == RestaurantId);

            if (restaurant == null) return NotFound();

            var restaurantsDto = _mapper.Map<RestaurantDto>(restaurant);
            return Ok(restaurantsDto);

        }
    }
}
