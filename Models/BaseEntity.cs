using System;

namespace WeddingPlanner
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get; set;}
    }
}