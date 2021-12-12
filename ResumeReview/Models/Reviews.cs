using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResumeReview.Models
{
    public class Reviews
    {
        [Key]
        public int ReviewId { get; set; }

        public Resume Resume { get; set; }

        public string Review { get; set; }

        public bool IsActive { get; set; }

        public string ReviewType { get; set; }

        public DateTime ReviewDate { get; set; }

        public Guid ReviewerId { get; set; }
    }
}
