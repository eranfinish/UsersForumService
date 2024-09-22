using FluentValidation;
using UsersForumService.Models;

namespace UsersForumService.Validations
{
   //validatoins for UserController 

    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(user => user.UserName)
             .NotEmpty().WithMessage("UserName is required.")
              .Length(2, 50).WithMessage("UserName must be between 2 and 50 characters.");

            RuleFor(user => user.Password)
                           .NotEmpty().WithMessage("Password is required.")
                           .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        }


    }

}
