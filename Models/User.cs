using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class User: BaseEntity
    {
        [Key]
        public int UserId {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Email {get; set;}
        public string Password {get; set;}
        public List <Wedding> WeddingsCreated {get;set;}
        public List <WeddingUser> WeddingsToGo {get;set;}
        
        public User()
        {
            WeddingsCreated = new List <Wedding>();
            WeddingsToGo = new List <WeddingUser>();
        }
        

    }
}