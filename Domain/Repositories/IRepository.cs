using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Repositories
{
	public interface IRepository
    {
        Task<IEnumerable<T>> Find<T>(int id);

        Task<int> Create<T>(T newObj);

        Task<int> Update<T>(T newObj, int id);

        Task<string> Remove(int id);
    }
}
