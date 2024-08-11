using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Utility
{
    public static class SD
    {
        //Roles
        public const string SuperAdminUser = "SuperAdmin";
        public const string BasicUser = "Basic";
        public const string AdminUser = "Admin";
        public const string ManagerUser = "Manager";
        public const string SupervisorUser = "Supervisor";
        public const string EmployeekUser = "Employee";
        public const string DriverUser = "Driver";
        public const string CustomerUser = "Customer";

        // status Booking
        public const string BookingDone = "Booking";                                //تم الحجز
        public const string DriverDone = "Driver";                                  //تم اسناده للسائق
        public const string CompletedDone = "Completed";                            //منفذ
        public const string CancelDone = "CancelBooking";                           //ملغي
        public const string DeleteDone = "DeleteBooking";                           //محذوف

        // Session
        public const string UserIdSession = "_SessionUserId";
        public const string UserSession = "_SessionUser";
        public const string BranchSession = "_SessionBranch";
        public const string UnfulfilledAppo = "_SessionUnfulfilledAppo";            //المواعيد المتعثرة

        // Branch
        public const string BranchManagement = "الادارة العليا";
    }
}
