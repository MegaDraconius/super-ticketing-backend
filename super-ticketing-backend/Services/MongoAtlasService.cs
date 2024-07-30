using MongoDB.Driver;
using super_ticketing_backend.Models;

namespace super_ticketing_backend.Services
{
    public static class MongoAtlasService
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbSettings = new DataBaseSettings.DataBaseSettings();
            configuration.GetSection("super-ticketing-backend").Bind(mongoDbSettings);

            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);

            // Registrar el cliente de MongoDB en el contenedor de servicios
            services.AddSingleton(mongoClient);

            // Registrar una instancia de base de datos
            // services.AddSingleton<IMongoDatabase>(mongoClient.GetDatabase(mongoDbSettings.DatabaseName));
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
            services.AddSingleton(mongoDatabase);
            
            services.AddSingleton(mongoDatabase.GetCollection<Users>(mongoDbSettings.UsersCollectionName));
            services.AddSingleton(mongoDatabase.GetCollection<ITGuys>(mongoDbSettings.ITGuysCollectionName));
            services.AddSingleton(mongoDatabase.GetCollection<Country>(mongoDbSettings.CountryCollectionName));
            services.AddSingleton(mongoDatabase.GetCollection<Tickets>(mongoDbSettings.TicketsCollectionName));

            return services;
        }
    }
}