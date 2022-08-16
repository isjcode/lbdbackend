using System;
using System.Collections.Generic;
using System.Text;

namespace lbdbackend.Core.Entities {
    public class JoinMoviewsReviews : BaseEntity {
        public int MovieID { get; set; }
        public Movie Movie { get; set; }

        public int ReviewID { get; set; }
        public Review Review { get; set; }

    }

}