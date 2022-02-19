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
	public abstract class AbstractGeneralRepository : AbstractRepository
    {
		/// <summary>
		/// The Database connection string
		/// </summary>
        string ConnectionString { get; }
        string TableName { get; }

        public AbstractGeneralRepository(string connectionString, string tableName) : base(connectionString, tableName)
        {
            ConnectionString = connectionString;
            TableName = tableName;
        }

        protected AbstractGeneralRepository()
        {
        }

        public override async Task<IEnumerable<T>> Find<T>(int id)
        {
            IEnumerable<T> results;

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                var sql = $@"SELECT 
							    *				
							FROM {TableName}
                            WHERE ACTIVE=1 " + (id == -1 ? "" : " AND ID = @Id");

                results = await connection.QueryAsync<T>(sql, new { Id = id });
            }

            return results;
        }
    }
}
