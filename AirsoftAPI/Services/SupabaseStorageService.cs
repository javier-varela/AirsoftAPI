using AirsoftAPI.Services.IServices;
using AirsoftAPI.Utilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Supabase;
using Supabase.Interfaces;

namespace AirsoftAPI.Services
{
    public class SupabaseStorageService : ISupabaseStorageService
    {
        private readonly Supabase.Client _supabase;
        private readonly Supabase.SupabaseOptions _options;
        private readonly string _API_KEY;
        private readonly string _API_URL;
        public SupabaseStorageService(IConfiguration configuration)
        {

            _API_KEY = configuration.GetValue<string>("SupabaseKEY");
            _API_URL = configuration.GetValue<string>("SupabaseURL");
            _options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            _supabase = new Supabase.Client(_API_URL, _API_KEY, _options);
            _supabase.InitializeAsync().Wait();
        }

        public Task DeleteAsync(ContainerEnum container, string bucketName)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string bucketName, string directoryName)
        {
            
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("El archivo no puede ser nulo o vacío.");
            }
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            string supabasePath = directoryName + "/" + imageFile.FileName;

            var fileOptions = new Supabase.Storage.FileOptions
            {
                CacheControl = "3600",
                Upsert = false
            };

            await _supabase.Storage.From(bucketName)
                .Upload(imageBytes, supabasePath, options: fileOptions);

            string url = _supabase.Storage.From(bucketName).GetPublicUrl(supabasePath);
            return url;
        }
    }
    internal class CreatedUploadSignedUrlResponse
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
