namespace super_ticketing_backend.Dto_s;

public class TicketUpdateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public string Photo { get; set; }
    public DateTime SolvedDate { get; set; }
    public string UserEmail { get; set; }
    public string ItGuyEmail { get; set; }
}