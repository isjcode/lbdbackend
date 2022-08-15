﻿using AutoMapper;
using lbdbackend.Core.Entities;
using lbdbackend.Service.DTOs.AccountDTOs;
using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace lbdbackend.Api.App.User.Controllers {
    [Route("api/[controller]")]
    [ApiController]


    public class AccountsController : ControllerBase {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IJWTManager _jwtManager;


        public AccountsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IJWTManager jwtManager) {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtManager = jwtManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO) {
            AppUser user = _mapper.Map<AppUser>(registerDTO);

            IdentityResult identityResult = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!identityResult.Succeeded) {
                return BadRequest(identityResult.Errors);
            }

            identityResult = await _userManager.AddToRoleAsync(user, "Member");

            return StatusCode(200);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO) {
            AppUser foundByEmail = await _userManager.FindByEmailAsync(loginDTO.EmailOrUsername);
            AppUser foundByUserName = await _userManager.FindByNameAsync(loginDTO.EmailOrUsername);

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

        [HttpGet]
        [Route("CheckToken")]
        [Authorize(Roles = "Superadmin, Admin, Member")]

        public async Task<IActionResult> CheckToken() {
            return Ok();
        }


        //[HttpGet]
        //public async Task<IActionResult> CreateRoles() {
        //    //initializing custom roles 
        //    string[] roleNames = { "Superadmin", "Admin", "Member" };
        //    IdentityResult roleResult;

        //    foreach (var roleName in roleNames) {
        //        var roleExist = await _roleManager.RoleExistsAsync(roleName);
        //        if (!roleExist) {
        //            //create the roles and seed them to the database: Question 1
        //            roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
        //        }
        //    }

        //    return Content("Done.");
        //}

        //[HttpGet]
        //public async Task<IActionResult> createsuperadmin() {
        //    //AppUser superAdmin = new AppUser();

        //    //superAdmin.Email = "lasauthr@protonmail.com";
        //    ////superAdmin.UserName = "Superadmin";
        //    //await _userManager.CreateAsync(superAdmin, "Supp123!");
        //    await _userManager.AddToRoleAsync(await _userManager.FindByEmailAsync("lasauthr@protonmail.com"), "Superadmin");

        //    return Content("Password changed.");

        //}

    }
}
