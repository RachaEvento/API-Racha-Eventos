namespace OrganizadorEventos.Interfaces.Services;

public interface IGoogleDriveService
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<Stream> DownloadFileAsync(string fileId);
}