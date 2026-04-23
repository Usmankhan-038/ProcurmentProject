namespace ProcurmentProject.Dto
{
    public class ResponseModel
    {
        public string Message { get; set; } = default!;
        public bool Success { get; set; } = default!;
        public Object? Data { get; set; } = null!;
        public int? Id { get; set; } = null;
    }
}
