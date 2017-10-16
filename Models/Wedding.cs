using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedding.Models
{
    public class Wedding : BaseEntity
    {
        public int Id {get; set;}
        

        public DateTime Date {get; set;}


        public string Address {get; set;}
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int WedderOneId {get; set;}
        public User WedderOne {get; set;}

        public int WedderTwoId {get; set;}
        public User WedderTwo {get; set;}

        public List<Guest> Guests {get; set;}

        public Wedding()
        {
            Guests = new List<Guest>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

    }
}