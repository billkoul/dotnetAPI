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
    }
}
