namespace CRUD_Dapper.Models
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

}

