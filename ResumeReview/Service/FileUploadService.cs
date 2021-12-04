using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ResumeReview.Service
{
    public class FileUploadService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _clientSecret;

        public FileUploadService(IHttpClientFactory httpClientFactory, string clientSecret)
        {
            _httpClientFactory = httpClientFactory;
            _clientSecret = clientSecret;
        }

        
        public string uploadFile( IFormFile formFile, string path, string uri)
        {

            ConsumeAPI consume = new ConsumeAPI(_httpClientFactory, _clientSecret);

            var response = consume.Upload(path, uri, formFile);

            var resp = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

            return resp["id"].ToString();
            
        }


        public string get_temp_link(string id, string uri)
        {
            string temp_link = string.Empty;

            ConsumeAPI consume = new ConsumeAPI(_httpClientFactory, _clientSecret);

            var response = consume.GetMetadata(id, uri);

            var resp = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

            return resp["link"].ToString();
        }

    }
}
