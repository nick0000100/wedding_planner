using System;
using System.ComponentModel.DataAnnotations;

namespace wedding.Models
{
    public class Guest : BaseEntity
    {
        public int Id {get; set;}

        public int UserId {get; set;}
        public User User {get; set;}

        public int WeddingId {get; set;}
        public Wedding Wedding {get; set;}
    }
}