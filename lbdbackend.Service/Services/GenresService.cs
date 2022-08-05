using AutoMapper;
using lbdbackend.Core.Entities;
using lbdbackend.Core.Repositories;
using lbdbackend.Service.DTOs.GenreDTOs;
using lbdbackend.Service.Exceptions;
using lbdbackend.Service.Interfaces;
using P225NLayerArchitectura.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace lbdbackend.Service.Services {
    public class GenresService : IGenresService {
        private readonly IMapper _mapper;
        private readonly IGenreRepository _repo;
        public GenresService(IGenreRepository repo, IMapper mapper) {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task Create(GenreCreateDTO genreCreateDTO) {
            if (await _repo.ExistsAsync(e => e.Name.ToLower() == genreCreateDTO.Name.ToLower())) {
                throw new AlreadyExistException($"Genre name \"{genreCreateDTO.Name}\" already exists.");
            }

            Genre genre = _mapper.Map<Genre>(genreCreateDTO);
            genre.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(genre);
            await _repo.CommitAsync();
        }

        public async Task DeleteOrRestore(int? id) {
            if (id == null) {
                throw new BadRequestException("ID can't be null."); 
            }
            if (!await _repo.ExistsAsync(e => e.ID == id)) {
                throw new ItemNotFoundException("ID not found.");
            }

            await _repo.RemoveOrRestore(id);
            await _repo.CommitAsync();
        }

        public async Task Update(int? id, GenreUpdateDTO genreUpdateDTO) {
            if (id == null) {
                throw new BadRequestException("Id can't be null."); 
            }
            if (id != genreUpdateDTO.ID) {
                throw new BadRequestException("IDs do not match."); 
            }

            Genre genre = await _repo.GetAsync(e => e.ID == genreUpdateDTO.ID);
            genre.Name = genreUpdateDTO.Name;
            genre.UpdatedAt = DateTime.UtcNow;

            if (genre == null) {
                throw new NullReferenceException();
            }

            _repo.CommitAsync();
        }

    }
}