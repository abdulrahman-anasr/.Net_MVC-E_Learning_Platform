using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcE_Learning.Models
{
    public class Content
    {
        public int ContentId { get; set;}
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }

        public string Type { get; set; }

        public int CourseId { get; set; }

        // The PDF to be stored on the DB
        public byte[]? pdfFile { get; set; }

        // Temporary field for file upload, not mapped to the database
        [NotMapped]
        public IFormFile? PdfUpload { get; set; }

    }
}
