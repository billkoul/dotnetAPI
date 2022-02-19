using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Domain.Repositories;
using Npgsql;

namespace Infrastructure.Repositories
{
	public class UploadRepository : AbstractRepository, IUploadRepository
    {
		/// <summary>
		/// The Database connection string
		/// </summary>
		public string ConnectionString { get; }
        public string TableName { get; }

        public UploadRepository(string connectionString, string tableName) : base(connectionString, tableName)
        {
            ConnectionString = connectionString;
            TableName = tableName;
        }

        public override async Task<IEnumerable<T>> Find<T>(int gseId)
        {
            IEnumerable<T> results;

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                var sql = $@"SELECT 
							    *				
							FROM {TableName}
                            WHERE ACTIVE=1 " + (gseId == -1 ? "" : " AND GseId = @GseId");

                results = await connection.QueryAsync<T>(sql, new { GseId = gseId });
            }

            return results;
        }
    }
}
