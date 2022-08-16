using lbdbackend.Service.DTOs.MovieDTOs;
using lbdbackend.Service.DTOs.PersonDTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace lbdbackend.Service.Interfaces {
    public interface IMovieService {
        Task Create(MovieCreateDTO movieCreateDTO);
        Task DeleteOrRestore(int? id);
        Task Update(int? id, MovieUpdateDTO movieUpdateDTO);
        Task<List<MovieGetDTO>> GetMovies();
        Task<MovieGetDTO> GetByID(int? id);
        Task<List<MovieGetDTO>> GetByStr(string str);


    }
}