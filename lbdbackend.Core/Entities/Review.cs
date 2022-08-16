using System;
using System.Collections.Generic;
using System.Text;

namespace lbdbackend.Core.Entities {
    public class Review : BaseEntity {
        public string Body { get; set; }
        public AppUser Owner { get; set; }
        public Movie Movie { get; set; }
        public int Score { get; set; }
    }
}
