namespace CRUD_Dapper.Models
{
    public class Branch: BaseEntity
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
