namespace super_ticketing_backend.Dto_s;

public class LoginDto
{
    public string? Id { get; set; }
    public string UserEmail { get; set; }
    public string Role { get; set; }
    public string AccessToken { get; set; }
}