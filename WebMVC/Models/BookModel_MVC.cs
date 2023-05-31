using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMVC.Models
{
    public class BookModel_MVC
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ISBN { get; set; }
        [ForeignKey("Person")]
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

        public BookModel_MVC() { }

        public BookModel_MVC(PersonModel_MVC person)
        {
            PersonId = person.Id;
            Person = person;
        }
    }
}
