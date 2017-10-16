using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedding.Models
{
    public class WeddingViewModel : BaseEntity
    {       
        [Required]
        [DataType(DataType.Date)]
        [CustomDateAttribute(ErrorMessage = "Invalid Date")]
        public DateTime Date {get; set;}

        [Required]
        public string Address {get; set;}

        [Required]
        public string WedderOneName {get; set;}

        [Required]
        public string WedderTwoName {get; set;}
    }
}