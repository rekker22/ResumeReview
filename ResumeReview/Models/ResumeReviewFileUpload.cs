using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResumeReview.Models
{
    public class ResumeReviewFileUpload
    {
        public IEnumerable<ResumeReviews> resumeReviews { get; set; }

        public IFormFile FileUpload { get; set; }
    }
}
