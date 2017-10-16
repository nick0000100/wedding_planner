using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedding.Models
{
    public class User : BaseEntity
    {
        public int Id {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Guest> Attending {get; set;}

        public User()
        {
            Attending = new List<Guest>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}