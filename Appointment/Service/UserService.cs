using Appointment.Data;
using Appointment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Appointment.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ApplicationDbContext _context;

        public UserService(IHttpContextAccessor httpContext, ApplicationDbContext context)
        {
            _httpContext = httpContext;
            _context = context;
        }

        public string GetUserId()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public bool IsAuthenticated()
        {
            return _httpContext.HttpContext.User.Identity.IsAuthenticated;
        }

        public async Task<ApplicationUser> GetUser()
        {
            string userId = GetUserId();
            var user = await _context.ApplicationUsers.FindAsync(userId);
            return user;
        }

        public async Task<Branches> GetBranch()
        {
            string userId = GetUserId();
            var user = await _context.ApplicationUsers.FindAsync(userId);
            var branch = await _context.Branches.FindAsync(user.BranchId);
            return branch;
        }

    }
}
