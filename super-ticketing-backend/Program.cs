using super_ticketing_backend.DataBaseSettings;
using super_ticketing_backend.Repositories;
using super_ticketing_backend.Services;
using super_ticketing_backend.Services.CountryService;
using super_ticketing_backend.Services.TicketService;
using super_ticketing_backend.Services.UserService;
using super_ticketing_backend.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<ITGuyService>();
builder.Services.AddSingleton<CountryService>();
builder.Services.AddSingleton<TicketService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>(provider =>
{
    var userService = provider.GetRequiredService<UserService>();
    return new UserRepository(userService.GetUsersCollection());
});
builder.Services.AddSingleton<ITicketRepository, TicketRepository>(provider =>
{
    var ticketService = provider.GetRequiredService<TicketService>();
    return new TicketRepository(ticketService.GetTikcetsCollection());
});
builder.Services.AddSingleton<IITGuyRepository, ITGuyRepository>(provider =>
{
    var itGuyService = provider.GetRequiredService<ITGuyService>();
    return new ITGuyRepository(itGuyService.GetItGuysCollection());
});
builder.Services.AddSingleton<ICountryRepository, CountryRepository>(provider =>
{
    var countryService = provider.GetRequiredService<CountryService>();
    return new CountryRepository(countryService.GetCountriesCollection());
});
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.Configure<DataBaseSettings>(
    builder.Configuration.GetSection("super-ticketing-backend"));
builder.Services.AddCors(options =>
{
    options.AddPolicy("Allowlocalhost4200", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200") //URL del front
            .AllowAnyMethod()
            .AllowCredentials()
            .AllowAnyHeader();
    });
});
builder.Services.AddMongoDb(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("Allowlocalhost4200");

app.MapControllers();

app.Run();