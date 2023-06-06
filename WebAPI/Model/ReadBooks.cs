using System.ComponentModel.DataAnnotations;

namespace WebAPI.Model
{
    public class ReadBooks
    {
        [Key]
        public int Id { get; set; }
        [Required]
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
        [StringLength(500)]
        public string Review { get; set; }
        public short PageNumber { get; set; }
        public short Published { get; set; }
        public DateTime Finished { get; set; }
    }
}
