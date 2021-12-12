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
using ResumeReview.Service;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace ResumeReview.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IConfiguration _configuration;

        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _context = applicationDbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        //string imageName = "DResume_300x250.JPG";

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

            if (file == null || file.Length == 0)
                return Content("file not selected");

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Resume> resume = _context.Resume.Where(x => x.UploaderId.Equals(Guid.Parse(userId))).Where(t => t.IsDeleted == false).OrderBy(o => o.VersionNumber);

            string ResumeName = file.FileName;

            var versionNumber = (resume.Count() == 0) ? 0 : resume.LastOrDefault().VersionNumber + 1;

            string ResumeFileExtension = Path.GetExtension(file.FileName);

            var ClientSecret = _configuration.GetSection("Storage")["ClientSecret"];

            var DriveName = _configuration.GetSection("Storage")["ResumeStorage"];

            var uri = _configuration.GetSection("APIs")["Upload"];

            var _fus = new FileUploadService(_httpClientFactory, ClientSecret);

            
            var id = _fus.uploadFile(file, DriveName+ ResumeName, uri);



            Resume r = new Resume{
                ResumeName = ResumeName,
                VersionNumber = versionNumber,
                FileSize = file.Length,
                DriveId = DriveName,
                DriveResumeId = id,
                IsDeleted = false,
                IsActive = true,
                UploadDate = DateTime.UtcNow,
                UploaderId = Guid.Parse(userId)
            };

            _context.Resume.Add(r);

            

            //if (file == null || file.Length == 0)
            //    return Content("file not selected");

            //var path = Path.Combine(
            //            Directory.GetCurrentDirectory(), "wwwroot/DummyImages",
            //            file.FileName);

            //using (var stream = new FileStream(path, FileMode.Create))
            //{
            //    await file.CopyToAsync(stream);
            //}

            await _context.SaveChangesAsync();

            //List<ResumeReviews> resumereviews = new List<ResumeReviews>();

            //foreach (var res in resume)
            //{
            //    IEnumerable<Reviews> reviews = _context.Reviews.Where(x => x.Resume.Equals(res));
            //    resumereviews.Add(new ResumeReviews
            //    {
            //        Reviews = reviews,
            //        Resume = res
            //    });
            //}

            //resumeReviewFileUploads.resumeReviews = resumereviews;

            return RedirectToAction("Index");

            //return View("Index", resumeReviewFileUploads);
        }

        public async Task<IActionResult> DisableAsync(int ResumeId)
        {
            var res = _context.Resume.Find(ResumeId);

            res.IsActive = false;

            _context.Resume.Update(res);

            

            await _context.SaveChangesAsync();

            //var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //IEnumerable<Resume> resume = _context.Resume.Where(x => x.UploaderId.Equals(Guid.Parse(userId))).Where(t => t.IsDeleted == false).OrderBy(o => o.VersionNumber);

            //List<ResumeReviews> resumereviews = new List<ResumeReviews>();

            //foreach (var r in resume)
            //{
            //    IEnumerable<Reviews> reviews = _context.Reviews.Where(x => x.Resume.Equals(r));
            //    resumereviews.Add(new ResumeReviews
            //    {
            //        Reviews = reviews,
            //        Resume = r
            //    });
            //}

            //ResumeReviewFileUpload resumeReviewFileUploads = new ResumeReviewFileUpload();

            //resumeReviewFileUploads.resumeReviews = resumereviews;

            return RedirectToAction("Index");

            //return View("Index", resumeReviewFileUploads);

        }
        public async Task<IActionResult> DeleteAsync(int ResumeId)
        {
            var res = _context.Resume.Find(ResumeId);

            res.IsDeleted = true;

            _context.Resume.Update(res);

            await _context.SaveChangesAsync();

            //var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //IEnumerable<Resume> resume = _context.Resume.Where(x => x.UploaderId.Equals(Guid.Parse(userId))).Where(t => t.IsDeleted == false).OrderBy(o => o.VersionNumber);

            //List<ResumeReviews> resumereviews = new List<ResumeReviews>();

            //foreach (var r in resume)
            //{
            //    IEnumerable<Reviews> reviews = _context.Reviews.Where(x => x.Resume.Equals(r));
            //    resumereviews.Add(new ResumeReviews
            //    {
            //        Reviews = reviews,
            //        Resume = r
            //    });
            //}

            //ResumeReviewFileUpload resumeReviewFileUploads = new ResumeReviewFileUpload();

            //resumeReviewFileUploads.resumeReviews = resumereviews;

            return RedirectToAction("Index");

            //return View("Index", resumeReviewFileUploads);

        }

        public async Task<IActionResult> EnableAsync(int ResumeId)
        {
            var res = _context.Resume.Find(ResumeId);

            res.IsActive = true;

            _context.Resume.Update(res);

            await _context.SaveChangesAsync();

            //var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //IEnumerable<Resume> resume = _context.Resume.Where(x => x.UploaderId.Equals(Guid.Parse(userId))).Where(t => t.IsDeleted == false).OrderBy(o => o.VersionNumber);

            //List<ResumeReviews> resumereviews = new List<ResumeReviews>();

            //foreach (var r in resume)
            //{
            //    IEnumerable<Reviews> reviews = _context.Reviews.Where(x => x.Resume.Equals(r));
            //    resumereviews.Add(new ResumeReviews
            //    {
            //        Reviews = reviews,
            //        Resume = r
            //    });
            //}

            //ResumeReviewFileUpload resumeReviewFileUploads = new ResumeReviewFileUpload();

            //resumeReviewFileUploads.resumeReviews = resumereviews;

            return RedirectToAction("Index");

            //return View("Index", resumeReviewFileUploads);

        }

        public IActionResult Resume()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Resume> resumes = _context.Resume.Where(x => x.UploaderId != Guid.Parse(userId)).Where(t => t.IsDeleted == false).Where(t => t.IsActive == true);

            Random r = new Random();

            //ViewData["ImageName"] = resumes.ElementAt(r.Next(resumes.Count())).Uri;

            ResRev resumereview = new ResRev();

            if(resumes.Count() == 0) {

                return RedirectToAction("NoResume");
            }

            resumereview.Resume = resumes.ElementAt(r.Next(resumes.Count()));

            var uri = _configuration.GetSection("APIs")["Temp_Url"];

            var ClientSecret = _configuration.GetSection("Storage")["ClientSecret"];

            var _fus = new FileUploadService(_httpClientFactory, ClientSecret);

            var temp_uri = _fus.get_temp_link(resumereview.Resume.DriveResumeId, uri);

            resumereview.Resume.Uri = temp_uri;

            return View(resumereview);
        }

        public IActionResult DetailedResume()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Resume> resumes = _context.Resume.Where(x => x.UploaderId != Guid.Parse(userId)).Where(t => t.IsDeleted == false).Where(t => t.IsActive == true);

            Random r = new Random();

            //ViewData["ImageName"] = resumes.ElementAt(r.Next(resumes.Count())).Uri;

            ResRev resumereview = new ResRev();

            if (resumes.Count() == 0)
            {

                return RedirectToAction("NoResume");
            }

            resumereview.Resume = resumes.ElementAt(r.Next(resumes.Count()));

            var uri = _configuration.GetSection("APIs")["Temp_Url"];

            var ClientSecret = _configuration.GetSection("Storage")["ClientSecret"];

            var _fus = new FileUploadService(_httpClientFactory, ClientSecret);

            var temp_uri = _fus.get_temp_link(resumereview.Resume.DriveResumeId, uri);

            resumereview.Resume.Uri = temp_uri;

            return View(resumereview);
        }

        public IActionResult NoResume()
        {
            return View();
        }

        public async Task<IActionResult> SubmitAsync(int ResumeId, ResRev resRev)
        {

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = _context.Resume.Find(ResumeId);

            var rev = new Reviews
            {
                Resume = res,
                Review = resRev.Review.Review,
                IsActive = true,
                ReviewType = "ReviewNotDetailed",
                ReviewDate = DateTime.UtcNow,
                ReviewerId = Guid.Parse(userId)
            };

            _context.Reviews.Add(rev);

            await _context.SaveChangesAsync();

            return RedirectToAction("Resume");
        }

        public async Task<IActionResult> SubmitDetailedAsync(int ResumeId, ResRev resRev)
        {

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = _context.Resume.Find(ResumeId);

            var rev = new Reviews
            {
                Resume = res,
                Review = resRev.Review.Review,
                IsActive = true,
                ReviewType = "ReviewDetailed",
                ReviewDate = DateTime.UtcNow,
                ReviewerId = Guid.Parse(userId)
            };

            _context.Reviews.Add(rev);

            await _context.SaveChangesAsync();

            return RedirectToAction("Resume");
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
