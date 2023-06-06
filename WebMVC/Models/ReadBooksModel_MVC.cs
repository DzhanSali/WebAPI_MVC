using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class ReadBooksModel_MVC
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ISBN { get; set; }
        public int PersonId { get; set; }
        public PersonModel_MVC Person { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(200)]
        [Required]
        public string Author { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(500)]
        public string Review { get; set; }
        [DisplayName("Pages")]
        public short PageNumber { get; set; }
        [DisplayName("Year")]
        public short Published { get; set; }
        public DateTime Finished { get; set; }
    }
}
