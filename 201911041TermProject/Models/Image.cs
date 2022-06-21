using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _201911041TermProject.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public byte[]? File { get; set; }
    }
}
