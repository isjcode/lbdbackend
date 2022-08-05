using AutoMapper;
using lbdbackend.Core.Entities;
using lbdbackend.Core.Repositories;
using lbdbackend.Service.DTOs.GenreDTOs;
using lbdbackend.Service.DTOs.PersonDTOs;
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
    public class PersonService : IPersonService {
        private readonly IMapper _mapper;
        private readonly IPersonRepository _repo;
        private readonly IProfessionRepository _professionRepo;
        private readonly IWebHostEnvironment _env;

        public PersonService(IPersonRepository repo, IMapper mapper, IWebHostEnvironment env, IProfessionRepository professionRepo) {
            _repo = repo;
            _mapper = mapper;
            _env = env;
            _professionRepo = professionRepo;
        }
        public async Task Create(PersonCreateDTO personCreateDTO) {
            if (await _repo.ExistsAsync(e => e.Name == personCreateDTO.Name)) {
                throw new AlreadyExistException($"Person name \"{personCreateDTO.Name}\" already exists.");
            }

            if (!await _professionRepo.ExistsAsync(e => e.ID == personCreateDTO.ProfessionID)) {
                throw new ItemNotFoundException($"{personCreateDTO.ProfessionID} is not a valid ID.");
            }

            Person person = _mapper.Map<Person>(personCreateDTO);

            if (personCreateDTO.File.CheckFileContentType("image/jpeg")) {
                throw new BadRequestException("Wrong file type.");
            }

            if (personCreateDTO.File.CheckFileSize(300)) {
                throw new BadRequestException("File too big.");
            }

            person.Image = await personCreateDTO.File.CreateFileAsync(_env, "Assets", "Images", "People");

            person.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(person);
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
        public async Task Update(int? id, PersonUpdateDTO personUpdateDTO) {
            if (id == null) {
                throw new BadRequestException("Id can't be null.");
            }
            if (id != personUpdateDTO.ID) {
                throw new BadRequestException("IDs do not match.");
            }

            if (!await _repo.ExistsAsync(e => e.ID == personUpdateDTO.ID)) {
                throw new BadRequestException("ID doesn't exist.");
            }

            Person person = await _repo.GetAsync(e => e.ID == personUpdateDTO.ID);
            person.Name = personUpdateDTO.Name;
            person.Description = personUpdateDTO.Description;
            person.Image = await personUpdateDTO.File.CreateFileAsync(_env, "Assets", "Images", "People");
            person.UpdatedAt = DateTime.UtcNow;


            if (person == null) {
                throw new NullReferenceException();
            }

            _repo.CommitAsync();
        }


    }
}