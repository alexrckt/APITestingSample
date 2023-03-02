using APITestingSample.Model;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Newtonsoft.Json;
using NuGet.Frameworks;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APITestingSample
{
    public class BaseTest
    {
        public RestClient client;
        protected Random rnd = new Random();

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            client = new RestClient("https://petstore.swagger.io/v2/");
        }

        protected RestResponse<T> GetRequest<T>(string path)
        {
            RestRequest request = new RestRequest(path, Method.Get);
            return client.ExecuteAsync<T>(request).Result;
        }

        protected RestResponse GetRequest(string path)
        {
            RestRequest request = new RestRequest(path, Method.Get);
            return client.ExecuteAsync(request).Result;
        }

        protected RestResponse DeleteRequest(string path)
        {
            RestRequest request = new RestRequest(path, Method.Delete);
            return client.ExecuteAsync(request).Result;
        }

        protected RestResponse<T> PostRequest<T>(string path, T param) where T : class
        {
            var request = new RestRequest(path, Method.Post);

            request.AddJsonBody(param);

            var resp = client.ExecuteAsync<T>(request).Result;

            return resp;
        }

        protected RestResponse<T> PutRequest<T>(string path, T param) where T : class
        {
            var request = new RestRequest(path, Method.Put);
            request.AddJsonBody(param);

            var resp = client.ExecuteAsync<T>(request).Result;

            return resp;
        }
    }

    public static class Extentions
    {
        public static T GetBody<T>(this RestResponse<T> response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public static ServiceResponse GetServiceResponse(this RestResponse response)
        {
            return JsonConvert.DeserializeObject<ServiceResponse>(response.Content);
        }
    }
}
