using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class PersonModel_MVC
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public short Age { get; set; }
        [Required]
        public string Address { get; set; }
        public char Gender { get; set; }
        public string PhoneNumber { get; set; }
    }
}
