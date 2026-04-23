using ProcurmentProject.Models;

namespace ProcurmentProject.Helper
{
    public class DocumentUploader
    {
        public async Task<Document> UploadDocument(int suppliesDeliveryId, string belongingName, IFormFile attachment)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var originalFileName = Path.GetFileName(attachment.FileName);
            var extension = Path.GetExtension(attachment.FileName);
            var encodedFileName = Guid.NewGuid().ToString() + extension;
            string fullPath = Path.Combine(folderPath, encodedFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await attachment.CopyToAsync(stream);
            }

            var document = new Document
            {
                BelongId = suppliesDeliveryId,
                BelongName = belongingName,
                EncodedFileName = encodedFileName,
                OriginalFileName = originalFileName,
                Url = folderPath
            };

            return document;
        }
    }
}
