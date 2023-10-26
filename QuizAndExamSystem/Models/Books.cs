using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSystem.Models
{
    public class Books : BaseEntity
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? FileName { get; set; }
        public string? FileThumbnail { get; set; }
        public string? BookType { get; set; }
        //public string? Publisher { get; set; }
        //public string? ISBN { get; set; }


    }
}
