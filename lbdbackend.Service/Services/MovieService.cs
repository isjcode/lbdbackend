using AutoMapper;
using lbdbackend.Core.Entities;
using lbdbackend.Core.Repositories;
using lbdbackend.Service.DTOs.GenreDTOs;
using lbdbackend.Service.DTOs.MovieDTOs;
using lbdbackend.Service.Exceptions;
using lbdbackend.Service.Interfaces;
using P225Allup.Extensions;
using P225NLayerArchitectura.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace lbdbackend.Service.Services {
    public class MovieService : IMovieService {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _repo;
        public MovieService(IMovieRepository repo, IMapper mapper) {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task Create(MovieCreateDTO movieCreateDTO) {
            if (await _repo.ExistsAsync(e => e.Name.ToLower() == movieCreateDTO.Name.ToLower())) {
                throw new AlreadyExistException($"Movie name \"{movieCreateDTO.Name}\" already exists.");
            }

            Movie movie = _mapper.Map<Movie>(movieCreateDTO);
            if (movieCreateDTO.BackgroundImage.CheckFileContentType("image/jpeg") || movieCreateDTO.PosterImage.CheckFileContentType("image/jpeg") {
                throw new BadRequestException("Wrong file type.");
            }

            if (movieCreateDTO.BackgroundImage.CheckFileSize(300) || movieCreateDTO.PosterImage.CheckFileSize(300)) {
                throw new BadRequestException("File too big.");
            }

            movie.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(movie);
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

    }
}