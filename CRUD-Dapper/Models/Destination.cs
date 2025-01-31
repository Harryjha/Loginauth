﻿namespace CRUD_Dapper.Models
{
    public class Destination
    {
        public int Id { get; set; } // Ensure ID exists
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int OwnerId { get; set; }
    }
}

