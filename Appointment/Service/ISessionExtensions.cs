using Appointment.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Service
{
    public interface ISessionExtensions
    {
        string GetUserId(ISession session);
        Task<ApplicationUser> GetUser(ISession session);
        Task<Branches> GetBranch(ISession session);
        Task<int> GetCountUnfulfilledAppo(ISession session);
    }
}
