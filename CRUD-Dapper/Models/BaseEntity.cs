namespace CRUD_Dapper.Models
{
    public class BaseEntity
    {
        public int id { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

    }
}
