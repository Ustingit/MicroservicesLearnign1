using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service
{
	public static class Extensions
	{
		public static ItemDto AsDto(this Item entity)
		{
			return new ItemDto(entity.Id, entity.Name, entity.Description, entity.Price, entity.CreatedDate);
		}
	}
}
