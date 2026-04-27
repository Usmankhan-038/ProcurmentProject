using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProcurmentProject.Dto
{
    public class RfqDetailDto
    {
        public int RfqId { get; set; }
        public string? RfqStatus { get; set; }
        public string PrTitle { get; set; } = default!;
        public int PrQuantity { get; set; }
        public string? PrEstimatedBudget { get; set; }
        public DateTime? PrDeliveryDate { get; set; }
        public string? PrNote { get; set; }
        public string ProductName { get; set; } = default!;
        public string? ProductCompany { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductUPC { get; set; }
        [JsonIgnore]
        public string? Documents { get; set; }

        [NotMapped]
        public List<RfqDocumentDto> DocumentList
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Documents) || Documents == null)
                    return new List<RfqDocumentDto>();

                try
                {
                    return JsonSerializer.Deserialize<List<RfqDocumentDto>>(Documents, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,

                    }) ?? new List<RfqDocumentDto>();
                } catch
                {
                    return new List<RfqDocumentDto>();
                }
            }
        }


    }
}
