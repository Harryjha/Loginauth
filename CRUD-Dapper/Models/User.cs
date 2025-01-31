namespace CRUD_Dapper.Models
{
    public class User
    {
        public int Id { get; set; }  // Make sure this property exists
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

