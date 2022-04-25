using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Repositories
{
	public static class Extensions
	{
		public static IServiceCollection AddMongo(this IServiceCollection services)
		{
			BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
			BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

			services.AddSingleton(serviceProvider =>
			{
				var configuration = serviceProvider.GetService<IConfiguration>();

				var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
				var mongoDbSettings = configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();
				var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);

				return mongoClient.GetDatabase(serviceSettings.ServiceName);
			});

			return services;
		}

		public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) 
			where T : IEntity
		{
			services.AddSingleton<IRepository<T>>(serviceProvider =>
			{
				var database = serviceProvider.GetService<IMongoDatabase>();
				return new MongoRepository<T>(database, collectionName);
			});

			return services;
		}
	}
}
