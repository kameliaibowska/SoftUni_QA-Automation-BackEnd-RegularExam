using RestSharp;
using System.Net;
using System.Text.Json;

namespace RestSharpAPITests
{
    public class RestSharpAPI_Tests
    {
        private RestClient client;
        private const string baseUrl = "https://contactbook.kameliaibowska.repl.co/api";

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient(baseUrl);
        }

        [Test]
        public void Test_GetAllContacts_CheckFirstContactNames() 
        {
            // Arrange
            RestRequest request = new RestRequest("contacts", Method.Get);

            // Act
            RestResponse response = this.client.Execute(request);
            List<Contact>? contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts[0].FirstName, Is.EqualTo("Steve"));
            Assert.That(contacts[0].LastName, Is.EqualTo("Jobs"));
        }

        [Test]
        public void Test_GetContact_ByKeyword_Valid()
        {
            // Arrange
            RestRequest request = new RestRequest("contacts/search/albert", Method.Get);

            // Act
            RestResponse response = this.client.Execute(request);
            List<Contact>? contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts[0].FirstName, Is.EqualTo("Albert"));
            Assert.That(contacts[0].LastName, Is.EqualTo("Einstein"));
        }

        [Test]
        public void Test_GetContact_ByKeyword_Invalid()
        {
            // Arrange
            RestRequest request = new RestRequest("contacts/search/missing" + DateTime.Now.Ticks, Method.Get);

            // Act
            RestResponse response = this.client.Execute(request);
            List<Contact>? contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts, Is.Empty);
            Assert.That(response.Content, Is.EqualTo("[]"));
        }

        [Test]
        public void Test_CreateContacts_Data_Invalid()
        {
            // Arrange
            RestRequest request = new RestRequest("contacts", Method.Post);
            var contactBody = new
            {
                firstName = "Nikol",
                email = "nikol.j@gmail.com"
            };

            // Act
            request.AddBody(contactBody);
            RestResponse response = this.client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Last name cannot be empty!\"}"));
        }

        [Test]
        public void Test_CreateContacts_Data_Valid()
        {
            // Arrange
            RestRequest request = new RestRequest("contacts", Method.Post);
            var contactBody = new
            {
                firstName = "Peter",
                lastName = "Pan",
                email = "peter.pan@gmail.com",
                phone = "+1 900 555 666",
                comments = "New contact."
            };

            // Act
            request.AddBody(contactBody);
            RestResponse response = this.client.Execute(request);
            ContactObject? contactObject = JsonSerializer.Deserialize<ContactObject>(response.Content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(contactObject.Msg, Is.EqualTo("Contact added."));
            Assert.That(contactObject.Contact.Id, Is.GreaterThan(0));
            Assert.That(contactObject.Contact.FirstName, Is.EqualTo(contactBody.firstName));
            Assert.That(contactObject.Contact.LastName, Is.EqualTo(contactBody.lastName));
            Assert.That(contactObject.Contact.Email, Is.EqualTo(contactBody.email));
            Assert.That(contactObject.Contact.Phone, Is.EqualTo(contactBody.phone));
            Assert.That(contactObject.Contact.DateCreated, Is.Not.Empty);
            Assert.That(contactObject.Contact.Comments, Is.EqualTo(contactBody.comments));
        }
    }
}