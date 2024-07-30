namespace super_ticketing_backend.Dto_s;

public class UserDto
{
    public string? Id { get; set; }
    public string CountryCode { get; set; }
    public string CountryId { get; set; }
    public string UserEmail { get; set; }
    public string Pwd { get; set; }
    public string Role { get; set; }
}