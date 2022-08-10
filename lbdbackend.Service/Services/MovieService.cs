using AutoMapper;
using lbdbackend.Core.Entities;
using lbdbackend.Core.Repositories;
using lbdbackend.Service.DTOs.GenreDTOs;
using lbdbackend.Service.DTOs.MovieDTOs;
using lbdbackend.Service.Exceptions;
using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IYearRepository _yearRepository;
        private readonly IGenreRepository _repoGenres;
        private readonly IPersonRepository _repoPeople;
        private readonly IJoinMoviesGenresRepository _repoMoviesGenres;
        private readonly IJoinMoviesPeopleRepository _repoMoviesPeople;
        private readonly IWebHostEnvironment _env;
        public MovieService(IMovieRepository repo, IMapper mapper, IWebHostEnvironment env, IYearRepository yearRepository, IPersonRepository repoMoviesPeople, IGenreRepository repoMoviesGenres, IJoinMoviesGenresRepository joinMoviesGenresRepository, IJoinMoviesPeopleRepository joinMoviesPeopleRepository) {
            _repo = repo;
            _mapper = mapper;
            _env = env;
            _yearRepository = yearRepository;
            _repoPeople = repoMoviesPeople;
            _repoGenres = repoMoviesGenres;
            _repoMoviesPeople = joinMoviesPeopleRepository;
            _repoMoviesGenres = joinMoviesGenresRepository;
        }
        public async Task Create(MovieCreateDTO movieCreateDTO) {
            if (await _repo.ExistsAsync(e => e.Name.ToLower() == movieCreateDTO.Name.ToLower())) {
                throw new AlreadyExistException($"Movie name \"{movieCreateDTO.Name}\" already exists.");
            }
            if (!await _yearRepository.ExistsAsync(e => e.ID == movieCreateDTO.YearID)) {
                throw new ItemNotFoundException("Year ID not found.");
            }

            if (movieCreateDTO.BackgroundImage.CheckFileContentType("image/jpeg") || movieCreateDTO.PosterImage.CheckFileContentType("image/jpeg")) {
                throw new BadRequestException("Wrong file type.");
            }

            if (movieCreateDTO.BackgroundImage.CheckFileSize(300) || movieCreateDTO.PosterImage.CheckFileSize(300)) {
                throw new BadRequestException("File too big.");
            }

            foreach (int id in movieCreateDTO.Genres) {
                if (!await _repoGenres.ExistsAsync(g => g.ID == id)) {
                    throw new ItemNotFoundException("Genre ID not found.");
                }
            }
            foreach (int id in movieCreateDTO.People) {
                if (!await _repoPeople.ExistsAsync(g => g.ID == id)) {
                    throw new ItemNotFoundException("Person ID not found.");
                }
            }

            Movie movie = _mapper.Map<Movie>(movieCreateDTO);

            movie.PosterImage = await movieCreateDTO.PosterImage.CreateFileAsync(_env, "Assets", "Images", "Movies", "Posters");
            movie.BackgroundImage = await movieCreateDTO.BackgroundImage.CreateFileAsync(_env, "Assets", "Images", "Movies", "Backgrounds");

            movie.CreatedAt = DateTime.UtcNow;


            await _repo.AddAsync(movie);
            await _repo.CommitAsync();

            foreach (int id in movieCreateDTO.Genres) {
                JoinMoviesGenres row = new JoinMoviesGenres();
                row.MovieID = movie.ID;
                row.GenreID = id;
                await _repoMoviesGenres.AddAsync(row);
            }

            foreach (int id in movieCreateDTO.People) {
                JoinMoviesPeople row = new JoinMoviesPeople();
                row.MovieID = movie.ID;
                row.PersonID = id;
                await _repoMoviesPeople.AddAsync(row);
            }
            await _repoMoviesGenres.CommitAsync();
            await _repoMoviesPeople.CommitAsync();
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
        public async Task Update(int? id, MovieUpdateDTO movieUpdateDTO) {
            if (id == null) {
                throw new BadRequestException("ID can't be null.");
            }
            if (id != movieUpdateDTO.ID) {
                throw new BadRequestException("IDs do not match.");
            }

            if (!await _repo.ExistsAsync(e => e.ID == movieUpdateDTO.ID)) {
                throw new BadRequestException("ID doesn't exist.");
            }

            Movie movie = await _repo.GetAsync(e => e.ID == movieUpdateDTO.ID);
            movie.Name = movieUpdateDTO.Name;
            movie.Synopsis = movieUpdateDTO.Synopsis;
            movie.PosterImage = await movieUpdateDTO.PosterImage.CreateFileAsync(_env, "Assets", "Images", "Movies", "Posters");
            movie.BackgroundImage = await movieUpdateDTO.BackgroundImage.CreateFileAsync(_env, "Assets", "Images", "Movies", "Backgrounds");
            movie.YearID = movieUpdateDTO.YearID;
            movie.UpdatedAt = DateTime.UtcNow;

            await _repo.CommitAsync();

            foreach (var row in await _repoMoviesGenres.GetAllAsync(e => e.MovieID == movie.ID)) {
                row.IsDeleted = true;
            }

            foreach (var row in await _repoMoviesPeople.GetAllAsync(e => e.MovieID == movie.ID)) {
                row.IsDeleted = true;
            }

            foreach (int genreID in movieUpdateDTO.Genres) {
                JoinMoviesGenres row = new JoinMoviesGenres();
                row.MovieID = movie.ID;
                row.GenreID = genreID;
                await _repoMoviesGenres.AddAsync(row);
            }

            foreach (int personID in movieUpdateDTO.People) {
                JoinMoviesPeople row = new JoinMoviesPeople();
                row.MovieID = movie.ID;
                row.PersonID = personID;
                await _repoMoviesPeople.AddAsync(row);

                if (movie == null) {
                    throw new NullReferenceException();
                }
                await _repoMoviesGenres.CommitAsync();
                await _repoMoviesPeople.CommitAsync();

            }

        }
    }
}