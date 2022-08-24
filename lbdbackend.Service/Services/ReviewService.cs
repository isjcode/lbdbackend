using AutoMapper;
using lbdbackend.Core.Entities;
using lbdbackend.Core.Repositories;
using lbdbackend.Service.DTOs.MovieDTOs;
using lbdbackend.Service.DTOs.ReviewDTOs;
using lbdbackend.Service.Exceptions;
using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using P225NLayerArchitectura.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace lbdbackend.Service.Services {
    public class ReviewService : IReviewService {
        private readonly IReviewRepository _repo;
        private readonly IMovieRepository _movieRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public ReviewService(IReviewRepository repo, IMapper mapper, IMovieRepository movieRepo, UserManager<AppUser> userManager) {
            _repo = repo;
            _mapper = mapper;
            _movieRepo = movieRepo; 
            _userManager = userManager;
        }
        public async Task Create(ReviewCreateDTO reviewCreateDTO) {
            if (!await _movieRepo.ExistsAsync(e => e.ID == reviewCreateDTO.MovieID)) {
                throw new ItemNotFoundException($"Movie ID doesn't exist.");
            }
            if (await _userManager.FindByIdAsync(reviewCreateDTO.OwnerID) == null) {
                throw new ItemNotFoundException($"User ID doesn't exist.");
            }
            if (await _repo.ExistsAsync(r => r.Body == reviewCreateDTO.Body && r.Rating == reviewCreateDTO.Rating)) {
                throw new AlreadyExistException("Review already exists");
            }
            Review review = _mapper.Map<Review>(reviewCreateDTO);
            review.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(review);
            await _repo.CommitAsync();
        }

        public async Task<List<ReviewGetDTO>> GetMovieReviews(int? movieID) {
            if (movieID == null) {
                throw new ArgumentNullException();
            }
            if (!await _movieRepo.ExistsAsync(m => m.ID == movieID)) {
                throw new ItemNotFoundException("Movie ID not found.");
            }

            List<ReviewGetDTO> reviews = new List<ReviewGetDTO>();

            foreach (Review review in await _repo.GetAllAsync(e => !e.IsDeleted && e.MovieId == movieID && e.Body.Trim().Length > 0, "Owner")) {
                var dto = _mapper.Map<ReviewGetDTO>(review);
                dto.Username = review.Owner.UserName;
                dto.Image = review.Owner.Image;
                reviews.Add(dto);
            }

            return reviews;
        }
        public async Task<PaginatedListDTO<ReviewGetDTO>> GetPaginatedReviews(int movieID, int i) {
            List<ReviewGetDTO> reviewGetDTOs = new List<ReviewGetDTO>();
            foreach (var item in await _repo.GetAllAsync(c => !c.IsDeleted && movieID == c.MovieId, "Owner")) {
                var dto = _mapper.Map<ReviewGetDTO>(item);
                dto.Username = item.Owner.UserName;
                dto.Image = item.Owner.Image;
                reviewGetDTOs.Add(dto);
            }
            PaginatedListDTO<ReviewGetDTO> paginatedListDTO = new PaginatedListDTO<ReviewGetDTO>(reviewGetDTOs, i, 2);

            return paginatedListDTO;
        }

        public async Task<ReviewGetDTO> GetReview(int reviewID) {
            if (!await _repo.ExistsAsync(r => r.ID == reviewID)) {
                throw new ItemNotFoundException("Review not found.");
            }

            var review = await _repo.GetAsync(r => !r.IsDeleted && r.ID == reviewID, "Owner");
            var dto = _mapper.Map<ReviewGetDTO>(review);
            dto.Username = review.Owner.UserName;
            dto.Image = review.Owner.Image;
            return dto;
        }
    }
}
