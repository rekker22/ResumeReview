using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResumeReview.Models
{
    public class UserViewed
    {
        public int Id { get; set; }

        public Resume Resume { get; set; }

        public Guid UserViewedId { get; set; }

        public DateTime UserViewedDate { get; set; }
    }
}
