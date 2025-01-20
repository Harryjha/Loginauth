namespace CRUD_Dapper.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdaedDate { get; set; }
    }
}
