namespace super_ticketing_backend.Services.PhotoService;

public interface IPhotoService
{
    Task<string> SaveImageAsync(IFormFile photo);
}