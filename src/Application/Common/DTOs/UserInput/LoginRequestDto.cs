using Application.Common.Constants;
using FluentValidation;

namespace Application.Common.DTOs.UserInput
{
    public class LoginRequestDto
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }


    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(v => v.Username)
                .NotEmpty().WithMessage(ValidationResponses.Required)
                .NotNull().WithMessage(ValidationResponses.Required)
                .EmailAddress().WithMessage(ValidationResponses.InvalidEmailFormat);

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage(ValidationResponses.Required)
                .NotNull().WithMessage(ValidationResponses.Required)
                .MinimumLength(6).WithMessage(ValidationResponses.MinimumLength(6));
        }
    }
}
