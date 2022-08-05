using lbdbackend.Service.DTOs.GenreDTOs;
using lbdbackend.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using lbdbackend.Service.Interfaces;
using System;
using lbdbackend.Service.DTOs.ProfessionDTOs;

namespace lbdbackend.Api.App.Admin.Controllers {
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ProfessionsController : ControllerBase {
        private readonly IProfessionsService _professionService;
        public ProfessionsController(IProfessionsService professionService) {
            _professionService = professionService;
        }
        [HttpPut]
        [Route("Create")]
        public async Task<IActionResult> Create(ProfessionCreateDTO professionCreateDTO) {
            await _professionService.Create(professionCreateDTO);
            return StatusCode(201);
        }

        [HttpPost]
        [Route("DeleteOrRestore")]
        public async Task<IActionResult> DeleteOrRestore(int? id) {
            await _professionService.DeleteOrRestore(id);
            return StatusCode(200);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(int? id, ProfessionUpdateDTO professionUpdateDTO) {
            await _professionService.Update(id, professionUpdateDTO);
            return StatusCode(200);
        }

    }
}