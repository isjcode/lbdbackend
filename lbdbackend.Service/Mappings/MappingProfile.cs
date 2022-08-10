﻿using AutoMapper;
using lbdbackend.Core.Entities;
using lbdbackend.Service.DTOs.AccountDTOs;
using lbdbackend.Service.DTOs.GenreDTOs;
using lbdbackend.Service.DTOs.MovieDTOs;
using lbdbackend.Service.DTOs.PersonDTOs;
using lbdbackend.Service.DTOs.ProfessionDTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace lbdbackend.Service.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<RegisterDTO, IdentityUser>();

            CreateMap<GenreUpdateDTO, Genre>();
            CreateMap<Genre, GenreUpdateDTO>();
            CreateMap<Genre, GenreCreateDTO>();
            CreateMap<GenreCreateDTO, Genre>();
            CreateMap<Genre, GenreGetDTO>();
            CreateMap<GenreGetDTO, Genre>();

            CreateMap<ProfessionUpdateDTO, Profession>();
            CreateMap<Profession, ProfessionUpdateDTO>();
            CreateMap<Profession, ProfessionCreateDTO>();
            CreateMap<ProfessionCreateDTO, Profession>();
            CreateMap<Profession, ProfessionGetDTO>();
            CreateMap<ProfessionGetDTO, Profession>();

            CreateMap<Person, PersonCreateDTO>();
            CreateMap<PersonCreateDTO, Person>();
            CreateMap<Person, PersonGetDTO>();
            CreateMap<PersonGetDTO, Person>();

            CreateMap<Movie, MovieCreateDTO>();
            CreateMap<MovieCreateDTO, Movie>();
        }
    }
}
