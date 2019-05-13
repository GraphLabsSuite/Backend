using System.Security.Claims;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNetCore.Http;

namespace GraphLabs.Backend.Api.Auth
{
    internal sealed class UserInfoService : IUserInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal User => _httpContextAccessor?.HttpContext?.User;

        public string UserId => User?.FindFirstValue(ClaimTypes.Sid);

        public string UserRole => User?.FindFirstValue(ClaimTypes.Role);
    }
}