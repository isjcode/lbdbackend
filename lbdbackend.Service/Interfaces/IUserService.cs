using lbdbackend.Core.Entities;
using lbdbackend.Service.DTOs.GenreDTOs;
using lbdbackend.Service.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace lbdbackend.Service.Interfaces {
    public interface IUserService {
        Task<UserGetDTO> GetUserMain(string userName);
        Task Follow(string followerId, string followeeId);
        Task<bool> CheckFollow(string followerUsername, string followeeUsername);
    }
}