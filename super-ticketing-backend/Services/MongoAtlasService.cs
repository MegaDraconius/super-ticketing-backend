using MongoDB.Driver;

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
            services.AddSingleton<IMongoDatabase>(mongoClient.GetDatabase(mongoDbSettings.DatabaseName));

            return services;
        }
    }
}