using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using RestaurantApi.Entities;
using RestaurantApi.Exceptions;
using RestaurantApi.Models;

namespace RestaurantApi.Services
{
    public interface IDishService
    {
        void DeleteAll(int restaurantId)
        void Delete(int restaurantId, int dishId);
        List<DishDto> GetAll(int restaurantId);
        DishDto GetById(int restaurantId, int dishId);
        int Create(int restaurantId, CreateDishDto dto);
    }

    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<DishService> _logger;

        public DishService(RestaurantDbContext dbContext, IMapper mapper, ILogger<DishService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public void DeleteAll(int restaurantId)
        {
            _logger.LogError($"All dishes in Restaurant with id {restaurantId}: DELETE action invoked");

            var restaurant = GetRestaurantByIdWithDishes(restaurantId);

            _dbContext.RemoveRange(restaurant.Dishes);
            _dbContext.SaveChanges();
        }

        public void Delete(int restaurantId, int dishId)
        {
            _logger.LogError($"Dish with {dishId} in Restaurant with id {restaurantId}: DELETE action invoked");

            var restaurant = GetRestaurantById(restaurantId);

            var dish = _dbContext
                .Dishes
                .FirstOrDefault(d => d.Id == dishId);
            if (dish == null || dish.RestaurantId != restaurantId)
                throw new NotFoundException("Dish not found");

            _dbContext.Dishes.Remove(dish);
            _dbContext.SaveChanges();
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = GetRestaurantByIdWithDishes(restaurantId);

            var dishDto = _mapper.Map<List<DishDto>>(restaurant.Dishes);
            return dishDto;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _dbContext.Dishes.FirstOrDefault(d=>d.Id==dishId);
            if (dish == null || dish.RestaurantId!= restaurantId) 
                throw new NotFoundException("Dish not found");

            var dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }

        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _mapper.Map<Dish>(dto);

            dish.RestaurantId = restaurantId;

            _dbContext.Dishes.Add(dish);
            _dbContext.SaveChanges();

            return dish.Id;
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant == null)
                throw new NotFoundException("Restaurant not found");
            return restaurant;
        }

        private Restaurant GetRestaurantByIdWithDishes(int restaurantId)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(d => d.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant == null)
                throw new NotFoundException("Restaurant not found");
            return restaurant;
        }
    }
}