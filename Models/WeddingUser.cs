using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeddingPlanner.Models;

namespace WeddingPlanner
{
    public class WeddingUser
    {
        [Key]
        public int WeddingGuestId {get;set;}
        // [ForeignKey("UserId")]
        public int UserId {get;set;}
        public User User {get; set;} 
        // [ForeignKey("WeddingId")]
        public int WeddingId {get;set;}
        public Wedding Wedding {get;set;}
    }
}