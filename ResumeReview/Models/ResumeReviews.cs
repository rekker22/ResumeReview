using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResumeReview.Models
{
    public class ResumeReviews
    {
        public Resume Resume { get; set; }

        public IEnumerable<Reviews> Reviews { get; set; } 
    }
}
