using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace lbdbackend.Service.DTOs.ReviewDTOs {
    public class ReviewGetDTO {
        public string Body { get; set; }
        public int Rating { get; set; }
        public int MovieID { get; set; }
        public string OwnerID { get; set; }


    }

    public class ReviewGetValidator : AbstractValidator<ReviewCreateDTO> {
        public ReviewGetValidator() {
            RuleFor(r => r.Body).MaximumLength(300).WithMessage("Maximum length is 300 symbols.");
            RuleFor(r => r.OwnerID).NotNull().WithMessage("Cannot be null.");
            RuleFor(r => r.MovieID).NotNull().WithMessage("Cannot be null.");
        }
    }
}