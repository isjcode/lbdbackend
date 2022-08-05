using lbdbackend.Service.DTOs.GenreDTOs;
using lbdbackend.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using lbdbackend.Service.Interfaces;
using System;

namespace lbdbackend.Api.App.Admin.Controllers {
    [Route("api/admin/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase {
        private readonly IGenresService _genreService;
        public GenresController(IGenresService genreService) {
            _genreService = genreService;   
        }
        [HttpPut]
        [Route("Create")]
        public async Task<IActionResult> Create(GenreCreateDTO genreCreateDTO) {
            await _genreService.Create(genreCreateDTO);
            return StatusCode(201);
        }

        [HttpPost]
        [Route("DeleteOrRestore")]
        public async Task<IActionResult> DeleteOrRestore(int? id) {
            await  _genreService.DeleteOrRestore(id);
            return StatusCode(200);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(int? id, GenreUpdateDTO genreUpdateDTO) {
            await _genreService.Update(id, genreUpdateDTO);
            return StatusCode(200);
        }

    }
}
