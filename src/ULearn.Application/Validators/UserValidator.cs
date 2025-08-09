

using FluentValidation;
using ULearn.Application.DTOs;
using ULearn.Domain.Interfaces;

namespace ULearn.Application.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{

    public CreateUserDtoValidator()
    {

        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name field cannot be empty");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name field cannot be empty");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email field cannot be empty")
        .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password cannot be empty.")
        .Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$")
        .WithMessage("Password must be at least 8 characters long and include an uppercase letter, number, and symbol.");

    }


    /* Usage for async 

    public UserDtoValidator(IUserRepository userRepository){

    RuleFor(x => x.Email)
        .NotEmpty()
            .WithMessage("Email is required.")
            .WithErrorCode("EmailRequired")
        .EmailAddress()
            .WithMessage("Invalid email format.")
            .WithErrorCode("InvalidEmailFormat")
        .MustAsync(async (email, _) =>
            !await userRepository.UserExistsByEmailAsync(email))
            .WithMessage("A user with this email already exists.")
            .WithErrorCode("EmailAlreadyExists");
    }


    **/

}

public class UserLoginDtoValidator : AbstractValidator<LoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email field cannot be empty")
      .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password field cannot be empty.");

    }
}