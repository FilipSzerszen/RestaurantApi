using Microsoft.AspNetCore.Authorization;

namespace RestaurantApi.Autorization
{
    public class MinimumRestaurantCreated : IAuthorizationRequirement
    {
        public MinimumRestaurantCreated(int minimumRestaurantCreated)
        {
            _minimumRestaurantCreated = minimumRestaurantCreated;
        }

        public int _minimumRestaurantCreated { get; }
    }
}
