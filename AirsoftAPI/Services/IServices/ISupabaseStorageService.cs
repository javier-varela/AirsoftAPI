using AirsoftAPI.Utilities;

namespace AirsoftAPI.Services.IServices
{
    public interface ISupabaseStorageService
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string bucketName, string directoryName);

        Task DeleteAsync(ContainerEnum container, string blobFileName);
    }
}
