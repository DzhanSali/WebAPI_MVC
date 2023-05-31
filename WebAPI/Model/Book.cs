using System.ComponentModel.DataAnnotations;


namespace WebAPI.Model
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string ISBN { get; set; }    
        public int PersonId { get; set; }
        public Person Person { get; set; }
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
