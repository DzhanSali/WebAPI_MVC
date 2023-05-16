using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class BookModel_MVC
    {
        [Key]
        public int Id { get; set; }
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
        public short PageNumber { get; set; }
        public DateTime Published { get; set; }
    }
}
