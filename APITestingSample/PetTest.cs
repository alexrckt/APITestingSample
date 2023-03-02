using APITestingSample.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;

namespace APITestingSample
{
    public class PetTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PetCreateTest()
        {
            var randomId = rnd.Next(100000, 999999);

            CreatePet(randomId);

            var response = GetRequest("/pet/" + randomId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void PetUpdateTest()
        {
            var randomId = rnd.Next(100000, 999999);

            CreatePet(randomId);

            var newName = "name" + randomId;
            var response = UpdatePetName(randomId, newName);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var pet = GetRequest<Pet>("/pet/" + randomId).GetBody();
            Assert.That(pet.Name, Is.EqualTo(newName));
        }

        [Test]
        public void PetDeleteTest()
        {
            var randomId = rnd.Next(100000, 999999);

            CreatePet(randomId);

            var response = GetPetRequest(randomId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = DeletePetRequest(randomId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = GetPetRequest(randomId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(response.GetServiceResponse().Message, Is.EqualTo("Pet not found"));
        }

        RestResponse CreatePet(int id)
        {
            var response = GetPetRequest(id);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(response.GetServiceResponse().Message, Is.EqualTo("Pet not found"));

            var pet = new Pet() { Id = id };
            response = PostRequest<Pet>("/pet", pet);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            return response;
        }

        RestResponse GetPetRequest(int id)
        {
            return GetRequest("/pet/" + id);
        }

        RestResponse DeletePetRequest(int id)
        {
            return DeleteRequest("/pet/" + id);
        }

        RestResponse UpdatePetName(int id, string name)
        {
            var pet = new Pet() { Id = id, Name = name };
            return PutRequest("/pet", pet);
        }
    }
}