using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _201911041TermProject.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime? Date { get; set; }

        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
    }
}
