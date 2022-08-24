﻿using lbdbackend.Service.DTOs.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace lbdbackend.Service.DTOs.UserDTOs {
    public class UserGetDTO {
        public int FilmCount { get; set; }
        public int ListCount { get; set; }
        public int FollowerCount { get; set; }
        public int FolloweeCount { get; set; }
        public List<ReviewGetDTO> RecentReviews { get; set; }

    }
}