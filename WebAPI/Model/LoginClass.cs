using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Model
{
    [NotMapped]
    public class LoginClass
    {
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
