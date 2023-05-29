using System.Linq;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using RestaurantApi.Entities;

namespace RestaurantApi.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(r => r.Password)
                .MinimumLength(8);

            RuleFor(p=>p.ConfirmPassword)
                .Equal(p=>p.Password);

            RuleFor(e => e.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u=>u.Email==value);
                    if(emailInUse) {
                        context.AddFailure("Email", "That email is taken");

                    }
                });
        }
    }
}
