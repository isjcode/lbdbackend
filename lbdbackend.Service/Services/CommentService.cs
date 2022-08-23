using AutoMapper;
using lbdbackend.Core.Entities;
using lbdbackend.Core.Repositories;
using lbdbackend.Service.DTOs.CommentDTOs;
using lbdbackend.Service.DTOs.GenreDTOs;
using lbdbackend.Service.Exceptions;
using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using P225NLayerArchitectura.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lbdbackend.Service.Services {
    public class CommentService : ICommentService {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _repo;
        private readonly IReviewRepository _reviewRepo;
        private readonly UserManager<AppUser> _userManager;
        public CommentService(ICommentRepository repo, IMapper mapper, IReviewRepository reviewRepo, UserManager<AppUser> userManager) {
            _repo = repo;
            _mapper = mapper;
            _reviewRepo = reviewRepo;
            _userManager = userManager;
        }

        public async Task CreateComment(CommentCreateDTO commentCreateDTO) {
            if (!await _reviewRepo.ExistsAsync(e => e.ID == commentCreateDTO.ReviewID)) {
                throw new ItemNotFoundException("Review ID doesn't exist.");
            }
            if (await _userManager.FindByIdAsync(commentCreateDTO.OwnerId) == null) {
                throw new ItemNotFoundException("User ID doesn't exist.");
            }
            Comment comment = _mapper.Map<Comment>(commentCreateDTO);

            await _repo.AddAsync(comment);
            await _repo.CommitAsync();
        }
    }
}