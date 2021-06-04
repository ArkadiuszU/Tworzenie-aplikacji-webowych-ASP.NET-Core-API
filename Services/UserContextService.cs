using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public interface IUserContextService
    {
        public ClaimsPrincipal User { get; }
        public int? GetUserId { get; }
    }
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal User { get { return _httpContextAccessor.HttpContext?.User; } }
        public int? GetUserId { get { return (User is null) ? null : int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value); }}
    }
}
