using ProcurmentProject.Models;
using System.Globalization;

namespace ProcurmentProject.Helper
{
    public class DocumentUploader
    {
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg", ".tif", ".tiff",
            ".pdf",
            ".doc", ".docx"
        };

        private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp", "image/svg+xml", "image/tiff",
            "application/pdf",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };

        public async Task<Document> UploadDocument(int suppliesDeliveryId, string belongingName, IFormFile attachment)
        {
            if (attachment == null || attachment.Length == 0)
            {
                throw new InvalidDataException("Please provide a valid file.");
            }

            var extension = Path.GetExtension(attachment.FileName);
            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
            {
                throw new InvalidDataException("Invalid file type. Only images, PDF, and Word files are allowed.");
            }

            if (!string.IsNullOrWhiteSpace(attachment.ContentType) && !AllowedContentTypes.Contains(attachment.ContentType))
            {
                throw new InvalidDataException("Invalid file content type. Only images, PDF, and Word files are allowed.");
            }

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var originalFileName = Path.GetFileName(attachment.FileName);
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
