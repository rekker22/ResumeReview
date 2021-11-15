using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResumeReview.Models
{
    public class ResRev
    {

        public Resume Resume { get; set; }

        public Reviews Review { get; set; }

        public bool Empty
        {
            get
            {
                return (Resume.ResumeId == 0 &&
                        string.IsNullOrWhiteSpace(Resume.ResumeName) &&
                        string.IsNullOrWhiteSpace(Resume.Uri));
            }
        }
    }
}
