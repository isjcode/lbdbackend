using lbdbackend.Core.Entities;
using lbdbackend.Service.DTOs.AccountDTOs;
using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace lbdbackend.Api.App.Admin.Controllers {
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJWTManager _jwtManager;

        public AccountsController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IJWTManager jwtManager) {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtManager = jwtManager;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO) {
            IdentityUser foundByEmail = await _userManager.FindByEmailAsync(loginDTO.EmailOrUsername);
            IdentityUser foundByUserName = await _userManager.FindByNameAsync(loginDTO.EmailOrUsername);

            if (foundByEmail != null && await _userManager.CheckPasswordAsync(foundByEmail, loginDTO.Password)) {
                var token = await _jwtManager.GenerateToken(foundByEmail);
                return Ok(new { 
                    token = token 
                });
            }
            else if (foundByUserName != null && await _userManager.CheckPasswordAsync(foundByUserName, loginDTO.Password)) {
                var token = await _jwtManager.GenerateToken(foundByUserName);
                return Ok(new {
                    token = token
                });
            }


            return NotFound("Your credentials don’t match. It’s probably attributable to human error."); 
        }


    }
}
