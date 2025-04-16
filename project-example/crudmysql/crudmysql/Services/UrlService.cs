using crudmysql.Services.Interfaces;

namespace crudmysql.Services
{
    public class UrlService : IUrlService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UrlService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetImageUrl(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return null;

            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return relativePath;

            var baseUrl = $"{request.Scheme}://{request.Host}";
            return $"{baseUrl}/{relativePath}";
        }
    }
}
