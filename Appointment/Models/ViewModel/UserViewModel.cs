using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models.ViewModel
{
    public class UserViewModel
    {
        public List<ApplicationUser> ApplicationUsersList { get; set; }
        public List<Branches> BranchesList { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset LockoutEnd { get; set; }
        public string UserType { get; set; }
        public string BranchName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
