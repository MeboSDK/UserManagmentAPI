    using Application.DTOs;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Please ensure that to set a value for {PropertyName}")
                .Must(BeValidName).WithMessage("Please ensure that to set a valid value for {PropertyName}")
                .Length(3, 30);

            RuleFor(x => x.Email).EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible);
        }

        private bool BeValidName(string name)
        {
            return name.All(Char.IsLetter);
        }
    }
}
