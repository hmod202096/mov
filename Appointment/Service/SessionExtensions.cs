using Appointment.Models;
using Appointment.Repositories;
using Appointment.Utility;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Service
{
    public class SessionExtensions : ISessionExtensions
    {
        private readonly IUserService _userService;
        private readonly IReservationRepository _reservationRepository;

        public SessionExtensions(IUserService userService, IReservationRepository reservationRepository)
        {
            _userService = userService;
            _reservationRepository = reservationRepository;
        }

        public string GetUserId(ISession session)
        {

            if (string.IsNullOrEmpty(session.GetString(SD.UserIdSession)))
            {
                string user = _userService.GetUserId();

                session.SetString(SD.UserIdSession, user);
            }

            var userId = session.GetString(SD.UserIdSession);

            return userId.ToString();
        }

        public async Task<ApplicationUser> GetUser(ISession session)
        {

            if (string.IsNullOrEmpty(session.GetString(SD.UserSession)))
            {
                var getuser = await _userService.GetUser();

                session.SetString(SD.UserSession, JsonConvert.SerializeObject(getuser));
            }

            var user = JsonConvert.DeserializeObject<ApplicationUser>(session.GetString(SD.UserSession));

            return user;
        }

        public async Task<Branches> GetBranch(ISession session)
        {

            if (string.IsNullOrEmpty(session.GetString(SD.BranchSession)))
            {
                var branch = await _userService.GetBranch();

                session.SetString(SD.BranchSession, JsonConvert.SerializeObject(branch));
            }

            var branches = JsonConvert.DeserializeObject<Branches>(session.GetString(SD.BranchSession));

            return branches;
        }

        //عدد المواعيد المتعثره
        public async Task<int> GetCountUnfulfilledAppo(ISession session)
        {

            if (string.IsNullOrEmpty(session.GetString(SD.UnfulfilledAppo)))
            {
                var branch = await GetBranch(session);

                var countApp = _reservationRepository.UnfulfilledAppoRepo(branch.Id);

                session.SetInt32(SD.UnfulfilledAppo, countApp);
            }

            var count = session.GetInt32(SD.UnfulfilledAppo);

            return count.Value;
        }
    }
}
