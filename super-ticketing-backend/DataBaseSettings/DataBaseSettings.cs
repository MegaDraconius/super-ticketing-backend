namespace super_ticketing_backend.DataBaseSettings;

public class DataBaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;
    
    public string UsersCollectionName { get; set; } = null!;

    public string ITGuysCollectionName { get; set; } = null!;

    public string CountryCollectionName { get; set; } = null!;

}