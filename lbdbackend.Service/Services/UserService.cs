using AutoMapper;
using lbdbackend.Core.Entities;
using lbdbackend.Core.Repositories;
using lbdbackend.Service.DTOs.GenreDTOs;
using lbdbackend.Service.DTOs.UserDTOs;
using lbdbackend.Service.Exceptions;
using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using P225NLayerArchitectura.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lbdbackend.Service.Services {
    public class UserService : IUserService {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _repo;
        private readonly IRelationshipRepository _relationshipRepo;
        private readonly IReviewRepository _reviewRepo;
        public UserService(UserManager<AppUser> repo, IMapper mapper, IRelationshipRepository relationshipRepository, IReviewRepository reviewRepository) {
            _repo = repo;
            _mapper = mapper;
            _relationshipRepo = relationshipRepository;
            _reviewRepo = reviewRepository;
        }

        public async Task<bool> CheckFollow(string followerUsername, string followeeUsername) {
            if (followerUsername == followeeUsername) {
                throw new BadRequestException("Usernames can't be same.");
            }

            var follower = await _repo.FindByNameAsync(followerUsername);
            var followee = await _repo.FindByNameAsync(followeeUsername);

            if (follower == null) {
                throw new ItemNotFoundException("Follower id not found.");
            }

            if (followee == null) {
                throw new ItemNotFoundException("Followee id not found.");
            }
            if (await _relationshipRepo.ExistsAsync(r => r.FollowerId == follower.Id && r.FolloweeId == followee.Id)) {
                return true;
            }
            return false;
        }

        public async Task Follow(string followerUsername, string followeeUsername) {
            if (followerUsername == followeeUsername) {
                throw new BadRequestException("Usernames can't be same.");
            }

            var follower = await _repo.FindByNameAsync(followerUsername);
            var followee = await _repo.FindByNameAsync(followeeUsername);

            if (follower == null) {
                throw new ItemNotFoundException("Follower id not found.");
            }

            if (followee == null) {
                throw new ItemNotFoundException("Followee id not found.");
            }

            Relationship relationship = new Relationship();
            
            relationship.FollowerId = follower.Id;
            relationship.FolloweeId = followee.Id;

            await _relationshipRepo.AddAsync(relationship);
            await _relationshipRepo.CommitAsync();
        }

        public async Task<UserGetDTO> GetUserMain(string userName) {
            var user = await _repo.FindByNameAsync(userName);

            if (user == null) {
                throw new ItemNotFoundException("User not found.");
            }
            string userId = user.Id;
            
            int followeeCount = await _relationshipRepo.GetCount(e => e.FollowerId == userId);
            int followerCount = await _relationshipRepo.GetCount(e => e.FolloweeId == userId);


            UserGetDTO userGetDTO = new UserGetDTO();
            userGetDTO.FolloweeCount = followeeCount;
            userGetDTO.FollowerCount = followerCount;

            List<int> movieIds = new List<int>();

            foreach (Review review in await _reviewRepo.GetAllAsync(r => r.OwnerId == userId)) {
                movieIds.Add(review.MovieId);
            }

            int movieCount = movieIds.Distinct().Count();

            userGetDTO.FilmCount = movieCount;

            return userGetDTO;
        }

    }
}