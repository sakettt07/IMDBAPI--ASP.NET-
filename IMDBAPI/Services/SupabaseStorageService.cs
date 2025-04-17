using System;
using System.IO;
using System.Threading.Tasks;
using Supabase;
using Microsoft.Extensions.Configuration;
namespace IMDBAPI.Services
{
    public class SupabaseStorageService
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly string _bucketName = "moviescoverimage";

        public SupabaseStorageService(IConfiguration configuration)
        {
            var supabaseUrl = configuration["Supabase:SupabaseURL"];
            var supabaseKey = configuration["Supabase:SupabaseKey"];

            if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
            {
                throw new ArgumentException("Supabase credentials are missing from configuration.");
            }

            _supabaseClient = new Supabase.Client(supabaseUrl, supabaseKey, new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = false
            });
        }
        public async Task<string> UploadImageAsync(Stream stream, string fileName)
        {
            string filePath = $"{Guid.NewGuid()}{fileName}";
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            var storage = _supabaseClient.Storage.From(_bucketName);
            await storage.Upload(fileBytes, filePath);
            return _supabaseClient.Storage.From(_bucketName).GetPublicUrl(filePath);
        }
    }
}
