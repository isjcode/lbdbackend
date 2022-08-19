using System;
using System.Collections.Generic;
using System.Text;

namespace lbdbackend.Core.Entities {
    public class Comment : BaseEntity {
        public string Body { get; set; }
        public Review Review { get; set; }
    }
}
