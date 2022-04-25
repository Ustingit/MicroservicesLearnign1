﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
	public class ItemsRepository
	{
		private const string collectionName = "Items";

		private readonly IMongoCollection<Item> dbCollection;

		private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

		public ItemsRepository()
		{
			var mongoClient = new MongoClient("mongodb://localhost:27017");
			var db = mongoClient.GetDatabase("Catalog");

			dbCollection = db.GetCollection<Item>(collectionName);
		}

		public async Task<IReadOnlyCollection<Item>> GetAllAsync()
		{
			return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
		}

		public async Task<Item> GetAsync(Guid id)
		{
			FilterDefinition<Item> filter = filterBuilder.Eq(x => x.Id, id);

			return await dbCollection.Find(filter).FirstOrDefaultAsync();
		}

		public async Task CreateAsync(Item entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			await dbCollection.InsertOneAsync(entity);
		}

		public async Task UpdateAsync(Item entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			FilterDefinition<Item> filter = filterBuilder.Eq(x => x.Id, entity.Id);
			await dbCollection.ReplaceOneAsync(filter, entity);
		}

		public async Task DeleteAsync(Guid id)
		{
			FilterDefinition<Item> filter = filterBuilder.Eq(x => x.Id, id);
			await dbCollection.DeleteOneAsync(filter);
		}
	}
}