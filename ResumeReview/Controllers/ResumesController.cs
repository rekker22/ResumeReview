using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using ResumeReview.Data;
using ResumeReview.Models;

namespace ResumeReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        //static string[] Scopes = { DriveService.Scope.DriveFile };
        //static string ApplicationName = "ResumeReview";

        //private DriveService dservice = Authorize();

        //private static DriveService Authorize()
        //{
        //    UserCredential credential;

        //    using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
        //    {
        //        // The file token.json stores the user's access and refresh tokens, and is created
        //        // automatically when the authorization flow completes for the first time.
        //        string credPath = "token.json";
        //        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //            GoogleClientSecrets.FromStream(stream).Secrets,
        //            Scopes,
        //            "Admin",
        //            CancellationToken.None,
        //            new FileDataStore(credPath, true)).Result;
        //        Console.WriteLine("Credential file saved to: " + credPath);
        //    }

        //    // Create Drive API service.
        //    var service = new DriveService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = ApplicationName,
        //    });

        //    return service;
        //}

        //public Google.Apis.Drive.v3.Data.File uploadFile(string _uploadFile, string _parent, string _descrp = "Uploaded with .NET!")
        //{
        //    if (System.IO.File.Exists(_uploadFile))
        //    {
        //        Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
        //        body.Name = System.IO.Path.GetFileName(_uploadFile);
        //        body.Description = _descrp;
        //        body.MimeType = GetMimeType(_uploadFile);
        //        body.Parents = new List<string> { _parent };// UN comment if you want to upload to a folder(ID of parent folder need to be send as paramter in above method)
        //        byte[] byteArray = System.IO.File.ReadAllBytes(_uploadFile);
        //        System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
        //        try
        //        {
        //            FilesResource.CreateMediaUpload request = dservice.Files.Create(body, stream, GetMimeType(_uploadFile));
        //            request.SupportsTeamDrives = true;
        //            request.Upload();
        //            return request.ResponseBody;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message, "Error Occured");
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("The file does not exist.", "404");
        //        return null;
        //    }
        //}

        //private static string GetMimeType(string fileName)
        //{

        //    string contentType;
        //    new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
        //    return contentType ?? "application/octet-stream";

        //    //string mimeType = "application/unknown";
        //    //string ext = System.IO.Path.GetExtension(fileName).ToLower();
        //    //Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
        //    //if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
        //    //System.Diagnostics.Debug.WriteLine(mimeType);
        //    //return mimeType;
        //}



        public ResumesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Resumes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resume>>> GetResume()
        {
            return await _context.Resume.ToListAsync();
        }

        // GET: api/Resumes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Resume>> GetResume(int id)
        {
            var resume = await _context.Resume.FindAsync(id);

            if (resume == null)
            {
                return NotFound();
            }

            return resume;
        }

        // PUT: api/Resumes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResume(int id, Resume resume)
        {
            if (id != resume.ResumeId)
            {
                return BadRequest();
            }

            _context.Entry(resume).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResumeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Resumes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Resume>> PostResume(Resume resume)
        {
            _context.Resume.Add(resume);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResume", new { id = resume.ResumeId }, resume);
        }

        // DELETE: api/Resumes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResume(int id)
        {
            var resume = await _context.Resume.FindAsync(id);
            if (resume == null)
            {
                return NotFound();
            }

            _context.Resume.Remove(resume);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResumeExists(int id)
        {
            return _context.Resume.Any(e => e.ResumeId == id);
        }
    }
}
