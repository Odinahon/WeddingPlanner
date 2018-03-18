using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class Wedding :BaseEntity
    {
        [Key]
        public int WeddingId {get;set;}
        [Required]
        [MinLength(2)]
        public string WedderOne {get;set;}
        [Required]
        [MinLength(2)]
        public string WedderTwo {get;set;}
        [Required]
        [CheckFuture]
        public DateTime WeddingDate {get;set;}
        [Required]
        public string WeddingAddress {get;set;}
        [ForeignKey ("WhoCreateThisWedding")]
        public int UserId {get;set;}
        public User WhoCreateThisWedding{get;set;}
        public List <WeddingUser> Guests {get;set;}
        public Wedding()
        {
            Guests = new List<WeddingUser>();
        }
        

    }
}