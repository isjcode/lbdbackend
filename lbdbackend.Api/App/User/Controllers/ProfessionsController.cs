using lbdbackend.Service.DTOs.GenreDTOs;
using lbdbackend.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using lbdbackend.Service.Interfaces;
using System;
using lbdbackend.Service.DTOs.ProfessionDTOs;
using Microsoft.AspNetCore.Authorization;

namespace lbdbackend.Api.App.User.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionsController : ControllerBase {
        private readonly IProfessionsService _professionService;
        public ProfessionsController(IProfessionsService professionService) {
            _professionService = professionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            return Ok(await _professionService.GetProfessions());
        }

        [HttpGet]
        [Route("GetByID")]
        public async Task<IActionResult> GetByID(int? id) {
            return Ok(await _professionService.GetByID(id));
        }



    }
}