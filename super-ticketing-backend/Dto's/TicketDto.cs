namespace super_ticketing_backend.Dto_s;

public class TicketDto
{
    public string? Id { get; set; }
    public string TrackingId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ReportDate { get; set; }
    public DateTime SolvedDate { get; set; }
    public string Status { get; set; }
    public string Country { get; set; }
    public string Priority { get; set; }
    public string Photo { get; set; }
    public string UserId { get; set; }
    public string UserEmail { get; set; }
    public string ItGuyId { get; set; }
    public string ItGuyEmail { get; set; }
    
    public string Feedback { get; set; }
    public bool Stored { get; set; }
}