using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using RestaurantApi.Entities;
using RestaurantApi.Models;

namespace RestaurantApi.Services
{
    public interface IRestaurantService
    {
        bool Modify(int id, ModifyRestaurantDto modDto);
        bool Delete(int id);
        int Create(CreateRestaurantDto dto);
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto GetById(int id);
    }



    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public bool Modify(int id, ModifyRestaurantDto modDto)
        {
            var restaurant = _dbContext
                   .Restaurants
                   .FirstOrDefault(r => r.Id == id);

            if (restaurant == null) return false;

            restaurant.Name = modDto.Name;
            restaurant.Description = modDto.Description;
            restaurant.HasDelivery = modDto.HasDelivery;

            _dbContext.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            _logger.LogError($"Restaurant with id: {id} DELETE action invoked");

            var restaurant = _dbContext
                   .Restaurants
                   .FirstOrDefault(r => r.Id == id);
            if (restaurant == null) return false;

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
            return true;
        }

        public RestaurantDto GetById(int id)
        {
            var restaurants = _dbContext
               .Restaurants
               .Include(r => r.Address)
               .Include(d => d.Dishes)
               .FirstOrDefault(r => r.Id == id);

            if (restaurants == null) return null;

            var result = _mapper.Map<RestaurantDto>(restaurants);
            return result;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(d => d.Dishes)
                .ToList();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);
            return restaurantsDto;
        }

        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();
            return restaurant.Id;
        }
    }
}
