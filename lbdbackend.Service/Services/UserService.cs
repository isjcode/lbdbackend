using AutoMapper;
using lbdbackend.Core.Entities;
using lbdbackend.Core.Repositories;
using lbdbackend.Service.DTOs.GenreDTOs;
using lbdbackend.Service.DTOs.MovieDTOs;
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
        private readonly UserManager<AppUser> _userManager;
        public UserService(UserManager<AppUser> repo, IMapper mapper, IRelationshipRepository relationshipRepository, IReviewRepository reviewRepository, UserManager<AppUser> userManager) {
            _repo = repo;
            _mapper = mapper;
            _relationshipRepo = relationshipRepository;
            _reviewRepo = reviewRepository;
            _userManager = userManager;
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
            if (await _relationshipRepo.ExistsAsync(r => !r.IsDeleted && r.FollowerId == follower.Id && r.FolloweeId == followee.Id)) {
                return true;
            }
            return false;
        }

        public async Task<bool> Follow(string followerUsername, string followeeUsername) {
            if (followerUsername == followeeUsername) {
                throw new BadRequestException("Usernames can't be same.");
            }

            bool isFollowing = false;

            var follower = await _repo.FindByNameAsync(followerUsername);
            var followee = await _repo.FindByNameAsync(followeeUsername);

            if (follower == null) {
                throw new ItemNotFoundException("Follower id not found.");
            }

            if (followee == null) {
                throw new ItemNotFoundException("Followee id not found.");
            }
            
            Relationship relationship = new Relationship();
            if (await _relationshipRepo.ExistsAsync(r => r.FollowerId == follower.Id && r.FolloweeId == followee.Id)) {
                var row = _relationshipRepo.GetAsync(r => r.FollowerId == follower.Id && r.FolloweeId == followee.Id).Result;
                if (row.IsDeleted) {
                    row.IsDeleted = false;
                    isFollowing = true;
                }
                else {
                    row.IsDeleted = true;
                    isFollowing = false;
                }
            }
            else {
                relationship.FollowerId = follower.Id;
                relationship.FolloweeId = followee.Id;
                await _relationshipRepo.AddAsync(relationship);
                isFollowing = true;
            }
            

            await _relationshipRepo.CommitAsync();
            return isFollowing;
        }

        public async Task<PaginatedListDTO<UserGetDTO>> GetUserFollowers(string userName, int i) {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) {
                throw new ItemNotFoundException("User not found.");
            }

            List<UserGetDTO> userGetDTOs = new List<UserGetDTO>();

            var followers = await _relationshipRepo.GetAllAsync(f => !f.IsDeleted && f.FolloweeId == user.Id, "Follower");

            foreach (var f in followers) {
                var dto = new UserGetDTO();
                dto.Image = f.Follower.Image;
                dto.UserName = f.Follower.UserName;
                userGetDTOs.Add(dto);
            }

            PaginatedListDTO<UserGetDTO> paginatedListDTO = new PaginatedListDTO<UserGetDTO>(userGetDTOs, i, 5);


            return paginatedListDTO;
        }

        public async Task<PaginatedListDTO<UserGetDTO>> GetUserFollowees(string userName, int i) {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) {
                throw new ItemNotFoundException("User not found.");
            }

            List<UserGetDTO> userGetDTOs = new List<UserGetDTO>();

            var followees = await _relationshipRepo.GetAllAsync(f => !f.IsDeleted && f.FollowerId == user.Id, "Followee");

            foreach (var f in followees) {
                var dto = new UserGetDTO();
                dto.Image = f.Followee.Image;
                dto.UserName = f.Followee.UserName;
                userGetDTOs.Add(dto);
            }

            PaginatedListDTO<UserGetDTO> paginatedListDTO = new PaginatedListDTO<UserGetDTO>(userGetDTOs, i, 5);


            return paginatedListDTO;
        }

        public async Task<UserGetDTO> GetUserMain(string userName) {
            var user = await _repo.FindByNameAsync(userName);

            if (user == null) {
                throw new ItemNotFoundException("User not found.");
            }
            string userId = user.Id;
            
            int followeeCount = await _relationshipRepo.GetCount(e => !e.IsDeleted && e.FollowerId == userId);
            int followerCount = await _relationshipRepo.GetCount(e => !e.IsDeleted && e.FolloweeId == userId);


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