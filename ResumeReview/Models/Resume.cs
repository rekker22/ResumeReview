using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResumeReview.Models
{
    public class Resume
    {
        [Key]
        public int ResumeId { get; set; }

        public string ResumeName { get; set; }

        public int VersionNumber { get; set; }

        public long FileSize { get; set; }

        public string DriveResumeId { get; set; }

        public string DriveId { get; set; }

        public string Uri { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public DateTime UploadDate { get; set; }

        public Guid UploaderId { get; set; }
    }
}
