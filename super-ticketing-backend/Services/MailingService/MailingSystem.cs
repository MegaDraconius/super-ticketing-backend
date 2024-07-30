using System.Net.Mail;
using System.Net;

namespace super_ticketing_backend.Services.MailingService;

public class MailingSystem : IMailingSystem
{
    private string from = "alfacodersticketing@gmail.com";
    private string displayName = "alfaCoders";
    private string appPwd = "hkvuesoyletruxet";

    public MailMessage GenerateMailMessage(string body, string to, string about)
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(from, displayName);
        mail.To.Add(to);
        mail.Subject = about;
        mail.Body = body;
        mail.IsBodyHtml = true;

        return mail;
    }
    
    public SmtpClient GenerateSmtpClient(string host, int port)
    {
        SmtpClient client = new SmtpClient(host, port); //Aquí debes sustituir tu servidor SMTP y el puerto
        client.Credentials = new NetworkCredential(from, appPwd);
        client.EnableSsl = true;//En caso de que tu servidor de correo no utilice cifrado SSL, poner false

        return client;
    }
        
    public async Task SendCreationMail( string to, string about, string trackingId)
    {
        try
        {
            string body = $@"<h1>Nos complace informar que se ha reportado correctamente la siguiente incidencia: </h1></br>
                            <h2>{about} con tracking ID: {trackingId}</h2>
                            <p>Te iremos informando cuando el estado de tu incidencia sea modificada.</p>";

            MailMessage mail = GenerateMailMessage(body, to, about);
            SmtpClient client = GenerateSmtpClient("smtp.gmail.com", 587); //Aquí debes sustituir tu servidor SMTP y el puerto
    
            client.Send(mail);
        }
        catch (Exception ex)
        {
            // return NotFound();
        }
    }
    
    public async Task SendStatusUpdateMail(string to, string about, string newStatus, string newTitle, string feedback)
    {
        try
        {
            string body = $@"<h2>El estado de tu incidencia: '{newTitle}' ha sido modificado!</h2></br>
                            <p>Tu incicendia ahora se encuentra en: '{newStatus}'</p>
                            <p> Nuestro equipo técnico ha proporcionado el siguiente feedback: '{feedback}'</p>";

            MailMessage mail = GenerateMailMessage(body, to, about);
            SmtpClient client = GenerateSmtpClient("smtp.gmail.com", 587); //Aquí debes sustituir tu servidor SMTP y el puerto
    
            client.Send(mail);
        }
        catch (Exception ex)
        {
            // return NotFound();
        }
    }
}