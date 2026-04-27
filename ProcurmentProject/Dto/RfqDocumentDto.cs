using System.Text.Json.Serialization;

namespace ProcurmentProject.Dto
{
    public class RfqDocumentDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        public string? DocumentUrl { get; set; }
    }
}
