using AirsoftAPI.Utilities;

namespace AirsoftAPI.Services.IServices
{
    public interface IAzureStorageService
    {
        Task<string> UploadAsync(IFormFile file, ContainerEnum container, string bloblName = null);

        Task DeleteAsync(ContainerEnum container, string blobFileName);
    }
}
