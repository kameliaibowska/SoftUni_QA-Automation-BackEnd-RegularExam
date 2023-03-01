using System.Text.Json.Serialization;

namespace RestSharpAPITests
{
    public class ContactObject
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonPropertyName("contact")]
        public Contact Contact { get; set; }
    }
}