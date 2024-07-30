using super_ticketing_backend.Dto_s;

namespace super_ticketing_backend.Services.PhotoService;

public class PhotoService : IPhotoService
{
    public async Task<string> SaveImageAsync(IFormFile photo)
    {
        if (photo != null)
        {
            return string.Empty;
        }
        using var memoryStream = new MemoryStream();
        await photo.OpenReadStream().CopyToAsync(memoryStream);
        return Convert.ToBase64String(memoryStream.ToArray());
        
    }
}