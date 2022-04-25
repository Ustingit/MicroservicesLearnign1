using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
	public interface IItemsRepository
	{
		Task<IReadOnlyCollection<Item>> GetAllAsync();

		Task<Item> GetAsync(Guid id);

		Task CreateAsync(Item entity);

		Task UpdateAsync(Item entity);

		Task DeleteAsync(Guid id);
	}
}
