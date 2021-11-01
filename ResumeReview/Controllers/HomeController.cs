using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ResumeReview.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using ResumeReview.Data;
using Microsoft.AspNetCore.Identity;
using ResumeReview.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IO;

namespace ResumeReview.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = applicationDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        string imageName = "DResume_300x250.JPG";

        [HttpGet]
        public IActionResult Index()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Resume> resumes = _context.Resume.Where(x => x.UploaderId.Equals(Guid.Parse(userId))).Where(t => t.IsDeleted == false).OrderBy(o => o.VersionNumber);

            List<ResumeReviews> resumereviews = new List<ResumeReviews>();

            ResumeReviewFileUpload resumeReviewFileUploads = new ResumeReviewFileUpload();

            foreach(var r in resumes)
            {
                IEnumerable<Reviews> reviews = _context.Reviews.Where(x => x.Resume.Equals(r)).Where(t => t.IsActive == true);
                resumereviews.Add(new ResumeReviews{
                    Reviews = reviews,
                    Resume = r
                });
            }

            resumeReviewFileUploads.resumeReviews = resumereviews;

            return View(resumeReviewFileUploads);
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(ResumeReviewFileUpload resumeReviewFileUploads)
        {
            var file = resumeReviewFileUploads.FileUpload;

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Resume> resume = _context.Resume.Where(x => x.UploaderId.Equals(Guid.Parse(userId))).Where(t => t.IsDeleted == false).OrderBy(o => o.VersionNumber);

            string ResumeName = userId + "/" + (resume.Last().VersionNumber + 1).ToString();

            string ResumeFileExtension = Path.GetExtension(file.FileName);

            Resume r = new Resume{
                ResumeName = ResumeName,
                VersionNumber = resume.Last().VersionNumber + 1,
                FileSize = file.Length,
                Uri = file.FileName,
                IsDeleted = false,
                IsActive = true,
                UploadDate = DateTime.UtcNow,
                UploaderId = Guid.Parse(userId)
            };

            _context.Resume.Add(r);

            

            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/DummyImages",
                        file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await _context.SaveChangesAsync();

            List<ResumeReviews> resumereviews = new List<ResumeReviews>();

            foreach (var res in resume)
            {
                IEnumerable<Reviews> reviews = _context.Reviews.Where(x => x.Resume.Equals(res));
                resumereviews.Add(new ResumeReviews
                {
                    Reviews = reviews,
                    Resume = res
                });
            }

            resumeReviewFileUploads.resumeReviews = resumereviews;

            return View("Index", resumeReviewFileUploads);
        }

        public async Task<IActionResult> DisableAsync(int ResumeId)
        {
            var res = _context.Resume.Find(ResumeId);

            res.IsActive = false;

            _context.Resume.Update(res);

            

            await _context.SaveChangesAsync();

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Resume> resume = _context.Resume.Where(x => x.UploaderId.Equals(Guid.Parse(userId))).Where(t => t.IsDeleted == false).OrderBy(o => o.VersionNumber);

            List<ResumeReviews> resumereviews = new List<ResumeReviews>();

            foreach (var r in resume)
            {
                IEnumerable<Reviews> reviews = _context.Reviews.Where(x => x.Resume.Equals(r));
                resumereviews.Add(new ResumeReviews
                {
                    Reviews = reviews,
                    Resume = r
                });
            }

            ResumeReviewFileUpload resumeReviewFileUploads = new ResumeReviewFileUpload();

            resumeReviewFileUploads.resumeReviews = resumereviews;

            return View("Index", resumeReviewFileUploads);

        }
        public async Task<IActionResult> DeleteAsync(int ResumeId)
        {
            var res = _context.Resume.Find(ResumeId);

            res.IsDeleted = true;

            _context.Resume.Update(res);

            await _context.SaveChangesAsync();

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Resume> resume = _context.Resume.Where(x => x.UploaderId.Equals(Guid.Parse(userId))).Where(t => t.IsDeleted == false).OrderBy(o => o.VersionNumber);

            List<ResumeReviews> resumereviews = new List<ResumeReviews>();

            foreach (var r in resume)
            {
                IEnumerable<Reviews> reviews = _context.Reviews.Where(x => x.Resume.Equals(r));
                resumereviews.Add(new ResumeReviews
                {
                    Reviews = reviews,
                    Resume = r
                });
            }

            ResumeReviewFileUpload resumeReviewFileUploads = new ResumeReviewFileUpload();

            resumeReviewFileUploads.resumeReviews = resumereviews;

            return View("Index", resumeReviewFileUploads);

        }

        public async Task<IActionResult> EnableAsync(int ResumeId)
        {
            var res = _context.Resume.Find(ResumeId);

            res.IsDeleted = true;

            _context.Resume.Update(res);

            await _context.SaveChangesAsync();

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Resume> resume = _context.Resume.Where(x => x.UploaderId.Equals(Guid.Parse(userId))).Where(t => t.IsDeleted == false).OrderBy(o => o.VersionNumber);

            List<ResumeReviews> resumereviews = new List<ResumeReviews>();

            foreach (var r in resume)
            {
                IEnumerable<Reviews> reviews = _context.Reviews.Where(x => x.Resume.Equals(r));
                resumereviews.Add(new ResumeReviews
                {
                    Reviews = reviews,
                    Resume = r
                });
            }

            ResumeReviewFileUpload resumeReviewFileUploads = new ResumeReviewFileUpload();

            resumeReviewFileUploads.resumeReviews = resumereviews;

            return View("Index", resumeReviewFileUploads);

        }

        public IActionResult Resume()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Resume> resumes = _context.Resume.Where(x => x.UploaderId != Guid.Parse(userId)).Where(t => t.IsDeleted == false);

            Random r = new Random();

            ViewData["ImageName"] = resumes.ElementAt(r.Next(resumes.Count())).Uri;

            //ViewData["ImageName"] = imageName;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
