namespace CRUD_Dapper.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(string tableName);
        Task<T> GetById(string tableName, int id);
        Task Delete(string tableName, int id);
        Task Add(string tableName, T entity);
        Task Update(string tableName, T entity);
    }
}
