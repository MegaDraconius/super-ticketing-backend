namespace super_ticketing_backend.Services.MailingService;

public interface IMailingSystem
{
    Task SendCreationMail( string to, string about, string trackingId);
    Task SendStatusUpdateMail(string to, string about, string newStatus);
}