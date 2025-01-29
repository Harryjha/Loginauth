namespace CRUD_Dapper.Models
{
    public class Destination : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
