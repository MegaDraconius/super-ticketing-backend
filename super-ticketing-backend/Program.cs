using MongoDB.Driver;
using super_ticketing_backend.DataBaseSettings;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;
using super_ticketing_backend.Services;
using super_ticketing_backend.Utilities;
using System.Net.Mail;
using System.Net;
using super_ticketing_backend.Services.MailingService;
using super_ticketing_backend.Services.PhotoService;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUserRepository, UserRepository>(provider =>
{
    var userCollection = provider.GetRequiredService<IMongoCollection<Users>>();
    return new UserRepository(userCollection);
});
builder.Services.AddSingleton<ITicketRepository, TicketRepository>(provider =>
{
    var ticketCollection = provider.GetRequiredService<IMongoCollection<Tickets>>();
    return new TicketRepository(ticketCollection);
});
builder.Services.AddSingleton<IITGuyRepository, ITGuyRepository>(provider =>
{
    var itGuyCollection = provider.GetRequiredService<IMongoCollection<ITGuys>>();
    return new ITGuyRepository(itGuyCollection);
});
builder.Services.AddSingleton<ICountryRepository, CountryRepository>(provider =>
{
    var countryCollection = provider.GetRequiredService<IMongoCollection<Country>>();
    return new CountryRepository(countryCollection);
});
builder.Services.AddSingleton<ITicketStatusRepository, TicketStatusRepository>(provider =>
{
    var ticketStatusCollection = provider.GetRequiredService<IMongoCollection<TicketStatus>>();
    return new TicketStatusRepository(ticketStatusCollection);
});

builder.Services.AddScoped<IMailingSystem, MailingSystem>();

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
builder.Services.AddScoped<IPhotoService, PhotoService>();
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