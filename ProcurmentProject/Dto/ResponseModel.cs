using System.Text.Json.Serialization;

namespace ProcurmentProject.Dto
{
    public class ResponseModel
    {
        public string Message { get; set; } = default!;
        [JsonIgnore]
        public bool Success { get; set; } = default!;
        public Object? Data { get; set; } = null!;
        [JsonIgnore]
        public int? Id { get; set; } = null;
    }
}
