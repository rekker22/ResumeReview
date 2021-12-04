using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ResumeReview.Service
{
    public class ConsumeAPI
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _clientSecret;

        public ConsumeAPI(IHttpClientFactory httpClientFactory, string clientSecret)
        {
            _httpClientFactory = httpClientFactory;
            _clientSecret = clientSecret;
        }
        public string Upload(string path, string requesturi, IFormFile file)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requesturi);

            //httpRequestMessage.Headers.Add("Content-Type", "application/octet-stream");
            httpRequestMessage.Headers.Add("Dropbox-API-Arg", "{ \"path\": \""+ path +"\",\"mode\": \"add\",\"autorename\": true,\"mute\": false,\"strict_conflict\":false}");


            MultipartFormDataContent multiPartContent = new MultipartFormDataContent();
            ByteArrayContent byteArrayContent = new ByteArrayContent(GetBytes(file));
            byteArrayContent.Headers.Add("Content-Type", "application/octet-stream");
            multiPartContent.Add(byteArrayContent, "this is the name of the content", file.FileName);

            httpRequestMessage.Content = byteArrayContent;

            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _clientSecret);

            string responsestring = string.Empty;

            try
            {
                var httpResponseMessage = httpClient.Send(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    responsestring = httpResponseMessage.Content.ReadAsStringAsync().Result;

                }
                else
                {
                    responsestring = httpResponseMessage.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception e)
            {
                responsestring = e.Message;
            }

            return responsestring;
        }

        public static byte[] GetBytes(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }


        public string GetMetadata(string id, string requesturi)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requesturi);

           //httpRequestMessage.Headers.Add("Content-Type", "application/json");

            httpRequestMessage.Content = new StringContent("{ \"path\": \""+id+"\" }",Encoding.UTF8,"application/json");

            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _clientSecret);


            string responsestring = string.Empty;

            try
            {
                var httpResponseMessage = httpClient.Send(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    responsestring = httpResponseMessage.Content.ReadAsStringAsync().Result;

                }
                else
                {
                    responsestring = httpResponseMessage.Content.ReadAsStringAsync().Result;

                }
            }
            catch (Exception e)
            {
                responsestring = e.Message;
            }

            return responsestring;
        }



    }
}
