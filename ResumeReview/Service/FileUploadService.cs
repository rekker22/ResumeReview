using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResumeReview.Service
{
    public class FileUploadService
    {
        static string[] Scopes = { DriveService.Scope.DriveFile };
        static string ApplicationName = "ResumeReview";

        //private DriveService dservice = Authorize();


        

        private static DriveService Authorize(string ClientId, string ClientSecret)
        {
            UserCredential credential;

            // The file token.json stores the user's access and refresh tokens, and is created
            // automatically when the authorization flow completes for the first time.
            //string credPath = "token.json";
            string credPath = AppDomain.CurrentDomain.BaseDirectory + "token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = ClientId,
                        ClientSecret = ClientSecret
                    },
                    Scopes,
                    "admin",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        public string uploadFile(string ClientId, string ClientSecret, IFormFile formFile, string _parent = "ResumeStorage", string _descrp = "Uploaded with FileUploadService")
        {
            var _formFile = formFile;

            var dservice = Authorize(ClientId, ClientSecret);

            if (_formFile != null && _formFile.Length != 0)
            {
                Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
                var _uploadFile = _formFile.FileName;
                string fileId = string.Empty;
                body.Name = System.IO.Path.GetFileName(_uploadFile);
                body.Description = _descrp;
                body.MimeType = GetMimeType(_uploadFile);
                body.Parents = new List<string> { _parent };// UN comment if you want to upload to a folder(ID of parent folder need to be send as paramter in above method)
                //byte[] byteArray = System.IO.File.ReadAllBytes(_uploadFile);
                //System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                if (_formFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        _formFile.CopyTo(ms);

                        try
                        {
                            FilesResource.CreateMediaUpload request = dservice.Files.Create(body, ms, GetMimeType(_uploadFile));
                            request.SupportsTeamDrives = true;
                            request.Upload();
                            fileId = request.ResponseBody.Id;

                            return fileId;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message, "Error Occured");
                            return null;
                        }

                        //var fileBytes = ms.ToArray();
                        //string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                return fileId;
            }
            else
            {
                Console.WriteLine("The file does not exist.", "404");
                return null;
            }
        }

        private static string GetMimeType(string fileName)
        {

            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
            return contentType ?? "application/octet-stream";

            //string mimeType = "application/unknown";
            //string ext = System.IO.Path.GetExtension(fileName).ToLower();
            //Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            //if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
            //System.Diagnostics.Debug.WriteLine(mimeType);
            //return mimeType;
        }

    }
}
