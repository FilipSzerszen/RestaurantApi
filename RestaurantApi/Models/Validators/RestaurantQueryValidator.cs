using System.Linq;

using FluentValidation;

using RestaurantApi.Entities;

namespace RestaurantApi.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private int[] allowedPageSizes = new int[] { 5, 10, 15 };
        private string[] allowedSortByColumnNames = {
                nameof(Restaurant.Name), 
                nameof(Restaurant.Description), 
                nameof(Restaurant.Category) };
        public RestaurantQueryValidator(RestaurantDbContext dbContext)
        {
            
            RuleFor(r => r.SortBy).Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"SortBy is optional, must be in [{string.Join(",", allowedSortByColumnNames)}]");

            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);

            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", allowedPageSizes)}]");
                }
            });
        }
    }
}
