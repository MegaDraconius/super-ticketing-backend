using MongoDB.Driver;
using super_ticketing_backend.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace super_ticketing_backend.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly IMongoCollection<Tickets> _ticketsCollection;

    public TicketRepository(IMongoCollection<Tickets> ticketsCollection)
    {
        _ticketsCollection = ticketsCollection;
    }

    public async Task<List<Tickets>> GetAsync() =>
        await _ticketsCollection.Find(_ => true).ToListAsync();

    public async Task<Tickets?> GetAsync(string id) =>
        await _ticketsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Tickets newTicket) =>
        await _ticketsCollection.InsertOneAsync(newTicket);

    public async Task UpdateAsync(Tickets updatedTicket) =>
        await _ticketsCollection.ReplaceOneAsync(x => x.Id == updatedTicket.Id, updatedTicket);

    public async Task RemoveAsync(string id) =>
        await _ticketsCollection.DeleteOneAsync(x => x.Id == id);
    
    public async Task SendMail( string to, string about)
    {
        // string msge = "Error al enviar este correo. Por favor verifique los datos o intente más tarde.";
        string from = "alfacodersticketing@gmail.com";
        string displayName = "alfaCoders";
        try
        {
            string body = @"<style>
                            h1{color:dodgerblue;}
                            h2{color:darkorange;}
                            </style>
                            <h1>Has recibido una nueva incidencia.</h1></br>
                            <h2>Accede a Super Ticketing para consultarlo.</h2>";
            
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from, displayName);
            mail.To.Add(to);
            mail.Subject = about;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Aquí debes sustituir tu servidor SMTP y el puerto
            client.Credentials = new NetworkCredential(from, "hkvuesoyletruxet");
            client.EnableSsl = true;//En caso de que tu servidor de correo no utilice cifrado SSL, poner false
    
            client.Send(mail);
        }
        catch (Exception ex)
        {
            // return NotFound();
        }
        // return Ok();
    }
}

