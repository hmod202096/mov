using Appointment.Models.ViewModel;
using Appointment.Repositories;
using Appointment.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Utility
{
    public static class DataStorage
    {
        public static async Task<BookingListViewModel> GetBookingByBranchID(IReservationRepository reservationRepository,
                                                                            IUserService userService)
        {
            var branch = await userService.GetBranch();
            var branchId = branch.Id;
            var model = await reservationRepository.GetBooking(branchId);
            return model;
        }
    }
}
