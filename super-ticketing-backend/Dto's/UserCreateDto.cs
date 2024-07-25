namespace super_ticketing_backend.Dto_s;

public class UserCreateDto
{
    public string CountryCode { get; set; }
    public string UserEmail { get; set; }
    public string Pwd { get; set; }
    public string Role { get; set; }
}