using FluentValidation;

namespace lbdbackend.Service.DTOs.MovieDTOs {
    public class MovieCreateDTO {
        public string Name { get; set; }
        public string Synopsis { get; set; }
        public string BackgroundImage { get; set; }
        public string PosterImage { get; set; }
    }

    public class MovieCreateValidator : AbstractValidator<MovieCreateDTO> {
        public MovieCreateValidator() {
            RuleFor(r => r.Name)
                .MaximumLength(40).WithMessage("Maximum length is 40 symbols.")
                .NotEmpty().WithMessage("Cannot be empty.");
            RuleFor(r => r.Synopsis)
                .MaximumLength(40).WithMessage("Maximum length is 40 symbols.")
                .NotEmpty().WithMessage("Cannot be empty.");
            RuleFor(r => r.BackgroundImage)
                .MaximumLength(300).WithMessage("Maximum length is 300 symbols.")
                .NotEmpty().WithMessage("Cannot be empty.");
            RuleFor(r => r.BackgroundImage)
                .MaximumLength(300).WithMessage("Maximum length is 300 symbols.")
                .NotEmpty().WithMessage("Cannot be empty.");


        }

    }
}
