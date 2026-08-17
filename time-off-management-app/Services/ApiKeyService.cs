using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using time_off_management_app.Data;
using time_off_management_app.Models;

namespace time_off_management_app.Services
{
    public class ApiKeyService
    {
        private readonly ApplicationDbContext _context;

        public ApiKeyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiKey?> ValidateKeyAsync(String key)
        {
            var hash = HashKey(key);

            var apiKey = await _context.ApiKeys.FirstOrDefaultAsync(k => k.KeyHash.Equals(hash));

            if (apiKey == null || !apiKey.IsActive || (apiKey.ExpiresAt.HasValue && apiKey.ExpiresAt.Value < DateTime.Now))
            {
                return null;
            }

            return apiKey;
        }

        public static String HashKey(String key)
        {
            var bytes = SHA256.HashData(
                Encoding.UTF8.GetBytes(key));

            return Convert.ToHexString(bytes);
        }
    }
}
