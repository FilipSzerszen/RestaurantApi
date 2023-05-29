using System.Security.Claims;
using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using RestaurantApi.Entities;
using System.Linq;

namespace RestaurantApi.Autorization
{
    public class MinimumRestaurantCreatedHandler : AuthorizationHandler<MinimumRestaurantCreated>
    {
        public MinimumRestaurantCreatedHandler(RestaurantDbContext restaurantDbContext)
        {
            _restaurantDbContext = restaurantDbContext;
        }

        public RestaurantDbContext _restaurantDbContext { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRestaurantCreated requirement)
        {
            var userId = int.Parse(context.User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);
            var restaurantCount = _restaurantDbContext.Restaurants.Count(r => r.CreatedById == userId);

            if (restaurantCount >= requirement._minimumRestaurantCreated)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

    }
}
