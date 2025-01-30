namespace CRUD_Dapper.Models
{
    public class Destination
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Ensure it's not nullable
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty; // Add this line
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

