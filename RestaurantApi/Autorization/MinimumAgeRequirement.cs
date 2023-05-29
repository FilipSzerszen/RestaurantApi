using Microsoft.AspNetCore.Authorization;

namespace RestaurantApi.Autorization
{
    public class MinimumAgeRequirement: IAuthorizationRequirement
    {
        public MinimumAgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }

        public int MinimumAge { get; }

    }
}
