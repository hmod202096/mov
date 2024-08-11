using Appointment.Contantes;
using Appointment.Data;
using Appointment.Models;
using Appointment.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Seeds
{
    public static class DefaultBranch
    {
       
      
        public static async Task<int> SeedBranchAsync(ApplicationDbContext context)
        {
            var data = context.Branches.Where(m => m.Name == SD.BranchManagement).FirstOrDefault();
            if (data == null)
            {
                Branches branches = new Branches()
                {
                    Name = SD.BranchManagement,
                    PhoneNumber = "0112411998",
                    Message = "ضع رسالتك هنا .....",
                    MaxDate = 10
                };
                await context.Branches.AddAsync(branches);
                await context.SaveChangesAsync();
                return branches.Id;
            }
            return data.Id;
        }
        public static async Task SeedBranch_HistoryAsync(ApplicationDbContext context)
        {
            var branchId = await SeedBranchAsync(context);
            var data = await context.Branches_HistoryDates.Where(m => m.BranchId == branchId).ToListAsync();
            if (data.Count == 0)
            {
                var model = await context.HistoryDates.ToListAsync();
                foreach (var item in model)
                {
                    Branches_HistoryDates branches_HistoryDates = new Branches_HistoryDates()
                    {
                        BranchId = branchId,
                        HistoryDateId = item.Id,
                        CountBooking = 8
                    };
                    context.Branches_HistoryDates.Add(branches_HistoryDates);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
