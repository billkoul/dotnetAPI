using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Domain.Repositories;
using Domain.Models;
using Npgsql;

namespace Infrastructure.Repositories
{
	public abstract class AbstractRepository
    {
		/// <summary>
		/// The Database connection string
		/// </summary>
        string ConnectionString { get; }
        string TableName { get; }

        public AbstractRepository(string connectionString, string tableName)
        {
            ConnectionString = connectionString;
            TableName = tableName;
        }

        protected AbstractRepository()
        {
        }

        public abstract Task<IEnumerable<T>> Find<T>(int id);

        public async Task<int> Create<T>(T newObj)
        {
            int id;

            var props = newObj.GetType().GetProperties();
            var columnNames = props.Where(x => x.CanWrite).Select(p => p.Name).Where(x=> !x.Equals("Id")).ToList();

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                var sql = $@"INSERT INTO {TableName} (" + string.Join(",", columnNames) + @")
                                VALUES (@" + string.Join(",@", columnNames) + @") RETURNING id";

                id = await connection.QuerySingleAsync<int>(sql, newObj);
            }

            return id;
        }

        public async Task<int> Update<T>(T newObj, int id)
        {
            var props = newObj.GetType().GetProperties();
            var columnNames = props.Where(x => x.CanWrite).Select(p => p.Name).Where(x => !x.Equals("Id")).ToList();

            var set = columnNames.Select(col => col + " = @" + col).ToList();

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                var sql = $@"UPDATE {TableName} SET " + string.Join(",", set) + " WHERE id = @Id RETURNING id";

                return await connection.QuerySingleAsync<int>(sql, newObj);
            }
        }

        public async Task<string> Remove(int id)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                var sql = $@"UPDATE {TableName} SET ACTIVE = 0 WHERE id = @Id";

                await connection.QueryAsync(sql, new { Id = id });
            }

            return "ok";
        }
    }
}
