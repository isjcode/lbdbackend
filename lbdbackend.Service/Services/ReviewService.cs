using AutoMapper;
using lbdbackend.Core.Entities;
using lbdbackend.Core.Repositories;
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
            Review review = _mapper.Map<Review>(reviewCreateDTO);
            review.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(review);
            await _repo.CommitAsync();
        }
    }
}
