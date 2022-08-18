using lbdbackend.Service.DTOs.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace lbdbackend.Service.Interfaces {
    public interface IReviewService {
        Task Create(ReviewCreateDTO reviewCreateDTO);
        Task<List<ReviewGetDTO>> GetMovieReviews(int movieID);

    }
}