using Appointment.Repositories;
using Appointment.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Utility
{
    public static class TemplateGenerator
    {
        public static async Task<string> GetHTMLString(IReservationRepository reservationRepository,
                                                                            IUserService userService)
        {
            var reservtion = await DataStorage.GetBookingByBranchID(reservationRepository,userService);
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'>
                                <h1>قائمة المواعيد الحالية</h1>
                                </div>
                                <table align='center'>
                                    <tr>
                                        <th>اسم المتبرع</th>
                                        <th>الهاتف</th>
                                        <th>الحي</th>
                                        <th>تاريخ الموعد</th>
                                    </tr>");
            foreach (var emp in reservtion.ReservationsList)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", emp.Customers.Name, emp.Customers.Mobily, emp.Customers.Neighborhoods.Name, emp.AppointmentDate.ToShortDateString());
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
