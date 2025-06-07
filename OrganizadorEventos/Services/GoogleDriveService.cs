using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using OrganizadorEventos.Interfaces.Services;
using File = Google.Apis.Drive.v3.Data.File;

namespace OrganizadorEventos.Services;

public class GoogleDriveService : IGoogleDriveService
{
    private DriveService _driveService;

    public GoogleDriveService()
    {
        InitializeDrive().Wait();
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var fileMetadata = new File
        {
            Name = file.FileName,
            Parents = new List<string> { "18cIZh8XPGZ94BeWdGeVhx3ixTpFJ4unE" }
        };

        using var stream = file.OpenReadStream();

        var request = _driveService.Files.Create(fileMetadata, stream, file.ContentType);
        request.Fields = "id";
        await request.UploadAsync();

        var uploadedFile = request.ResponseBody;
        return uploadedFile.Id;
    }

    public async Task<Stream> DownloadFileAsync(string fileId)
    {
        var request = _driveService.Files.Get(fileId);
        var stream = new MemoryStream();
        await request.DownloadAsync(stream);
        stream.Position = 0; // Reset stream position to the beginning
        return stream;
    }

    private async Task InitializeDrive()
    {
        using var stream = new FileStream("service-account.json", FileMode.Open, FileAccess.Read);

        var credential = GoogleCredential.FromStream(stream)
            .CreateScoped(DriveService.Scope.DriveFile); // Use Drive if you need full access

        _driveService = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "RachaEventosAPI"
        });
    }
}