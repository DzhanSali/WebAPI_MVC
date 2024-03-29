﻿using System.ComponentModel.DataAnnotations;

namespace WebAPI.Model
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public short Age { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; }
        public char Gender { get; set; }
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        
    }
}
