using Microsoft.Extensions.Options;
using MongoDB.Driver;
using super_ticketing_backend.Models;

namespace super_ticketing_backend.Services.UserService;

public class ITGuyService
{
    private readonly IMongoCollection<ITGuys> _itGuysCollection;
    
    public ITGuyService(
        IOptions<DataBaseSettings.DataBaseSettings> itGuyStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            itGuyStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            itGuyStoreDatabaseSettings.Value.DatabaseName);

        _itGuysCollection = mongoDatabase.GetCollection<ITGuys>(
            itGuyStoreDatabaseSettings.Value.ITGuysCollectionName);
    }
    public IMongoCollection<ITGuys> GetItGuysCollection() => _itGuysCollection;
}