using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace lbdbackend.Service.Interfaces {
    public interface IJWTManager {

        Task<string> GenerateToken(IdentityUser user);
        string GetUsernameByToken(string token);
    }
}
