using FluentValidation;
using UsersForumService.Models;

namespace UsersForumService.Validations
{
    public class UserLoginValidator: AbstractValidator<UserLogin>
    {
        public UserLoginValidator()
        {
            RuleFor(user => user.UserName)
               .NotEmpty().WithMessage("UserName is required.")
                .Length(2, 50).WithMessage("UserName must be between 2 and 50 characters.");
            
            RuleFor(user => user.Password)
                           .NotEmpty().WithMessage("Password is required.")
                           .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
