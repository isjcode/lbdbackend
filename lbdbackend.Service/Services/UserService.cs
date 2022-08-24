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
        public UserService(UserManager<AppUser> repo, IMapper mapper, IRelationshipRepository relationshipRepository) {
            _repo = repo;
            _mapper = mapper;
            _relationshipRepo = relationshipRepository;
        }

        public async Task<UserGetDTO> GetUserMain(string userName) {
            var user = await _repo.FindByNameAsync(userName);

            if (user == null) {
                throw new ItemNotFoundException("User not found.");
            }
            string userId = user.Id;
            int? followerCount = null;
            int? followeeCount = null;

            await _relationshipRepo.GetAllAsync(e => e.FollowerId == userId).Result.Count;





            return _mapper.Map<UserGetDTO>(user);
        }

    }
}