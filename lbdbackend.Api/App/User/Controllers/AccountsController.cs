using AutoMapper;
using lbdbackend.Core.Entities;
using lbdbackend.Service.DTOs.AccountDTOs;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper) {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO) {
            IdentityUser user = _mapper.Map<IdentityUser>(registerDTO);

            IdentityResult identityResult = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!identityResult.Succeeded) {
                return BadRequest(identityResult.Errors);
            }

            identityResult = await _userManager.AddToRoleAsync(user, "Member");

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

        [HttpGet]
        public async Task<IActionResult> createsuperadmin() {
            //IdentityUser superAdmin = new IdentityUser();

            //superAdmin.Email = "lasauthr@protonmail.com";
            ////superAdmin.UserName = "Superadmin";
            //await _userManager.CreateAsync(superAdmin, "Supp123!");
            await _userManager.AddToRoleAsync(await _userManager.FindByEmailAsync("lasauthr@protonmail.com"), "Superadmin");

            return Content("Password changed.");

        }

    }
}
