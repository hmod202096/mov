using Appointment.Data;
using Appointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Seeds
{
    public static class DefaultHistoryDate
    {
        public static async Task SeedHistoryDateAsync(ApplicationDbContext context)
        {
            var data = context.HistoryDates.Count();

            if (data == 0)
            {
                DateTime StartDate = new DateTime(2022, 8, 4, 0, 0, 0, 0, DateTimeKind.Local).AddDays(-1);
                DateTime EndDate = DateTime.Now.Date.AddYears(1);
                int DayInterval = 1;
                int x = 0;

                while (StartDate.AddDays(DayInterval) <= EndDate)
                {
                    StartDate = StartDate.AddDays(DayInterval);
                    x += 1;

                    HistoryDate historyDate = new HistoryDate
                    {
                        Date = StartDate
                    };

                    context.HistoryDates.Add(historyDate);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
