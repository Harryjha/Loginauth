﻿using CRUD_Dapper.Data;
using Dapper;

namespace CRUD_Dapper.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IDapperContext _context;

        public GenericRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAll(string tableName)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = $"SELECT * FROM {tableName} order by CreatedDate desc";
                return await connection.QueryAsync<T>(query);
            }
        }

        public async Task<T> GetById(string tableName, int id)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = $"SELECT * FROM {tableName} WHERE Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<T>(query, new { Id = id });
            }
        }

        public async Task Delete(string tableName, int id)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = $"DELETE FROM {tableName} WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }


        public async Task Add(string _TableName, T _Entity)
        {
            using (var connection = _context.CreateConnection())
            {
                var _EntityTypeOf = typeof(T);
                var _GetProperties = _EntityTypeOf.GetProperties().Where(x => x.Name != "Id");
                DynamicParameters _DynamicParameters = new();

                foreach (var property in _GetProperties)
                {
                    var value = property.GetValue(_Entity);
                    _DynamicParameters.Add("@" + property.Name, value);
                }

                var idProperty = _EntityTypeOf.GetProperty("Id");
                if (idProperty != null)
                {
                    // ✅ No need for "p.Name != "id"" here since _GetProperties already excludes "Id"
                    string insertQuery = $"INSERT INTO {_TableName} ({string.Join(", ", _GetProperties.Select(p => p.Name))}) "
                         + $"VALUES ({string.Join(", ", _GetProperties.Select(p => "@" + p.Name))})";

                    await connection.ExecuteAsync(insertQuery, _DynamicParameters);
                }
                else
                {
                    throw new ArgumentException("Entity must have an 'Id' property.");
                }
            }
        }

  
        public async Task Update(string _TableName, T _Entity)
        {
            using (var connection = _context.CreateConnection())
            {
                var _EntityTypeOf = typeof(T);
                var _GetProperties = _EntityTypeOf.GetProperties();
                DynamicParameters _DynamicParameters = new();

                foreach (var property in _GetProperties)
                {
                    var value = property.GetValue(_Entity);
                    _DynamicParameters.Add("@" + property.Name, value);
                }

                var idProperty = _EntityTypeOf.GetProperty("Id");

                //var idProperty = _EntityTypeOf.GetProperty("Id") ?? _EntityTypeOf.GetProperty("id");
                if (idProperty != null || idProperty.PropertyType == typeof(int))
                {
                    string insertQuery = $"INSERT INTO {_TableName} ({string.Join(", ", _GetProperties.Where(p => p.Name != "Id").Select(p => p.Name))}) "
                    + $"VALUES ({string.Join(", ", _GetProperties.Where(p => p.Name != "Id").Select(p => "@" + p.Name))})";

                    await connection.ExecuteAsync(insertQuery, _DynamicParameters);
                }
                else
                {
                    throw new ArgumentException("Entity must have an 'Id' property.");
                }

            }
        }
        public async Task<T> GetByEmail(string tableName, string email)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = $"SELECT * FROM {tableName} WHERE Email = @Email";
                return await connection.QuerySingleOrDefaultAsync<T>(query, new { Email = email });
            }
        }

    }
}
