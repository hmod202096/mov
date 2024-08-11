using Appointment.Data;
using Appointment.Models;
using Appointment.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly ApplicationDbContext context;

        public BranchRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Branches>> GetAll()
        {
            var branches = await context.Branches.ToListAsync();

            return branches;
        }

        public async Task<int> AddBranch(Branches branches)
        {
            context.Branches.Add(branches);
            await context.SaveChangesAsync();
            return branches.Id;
        }

        public async Task<Branches> GetBranch(int? id)
        {
            var branch = await context.Branches.FindAsync(id);
            return branch;
        }

        public async Task<Branches> EditBranch(Branches model)
        {
            context.Branches.Update(model);
            await context.SaveChangesAsync();
            return model;
        }

        public async Task<Branches> DeleteBranch(Branches model)
        {
            context.Branches.Remove(model);
            await context.SaveChangesAsync();
            return model;
        }

        public async Task<int> AddBranchHistory(int id)
        {
            List<HistoryDate> historyDates = await context.HistoryDates.ToListAsync();

            foreach (var item in historyDates)
            {
                Branches_HistoryDates branches_HistoryDates = new Branches_HistoryDates()
                {
                    BranchId = id,
                    HistoryDateId = item.Id,
                    CountBooking = 30
                };

                context.Branches_HistoryDates.Add(branches_HistoryDates);
            }

            await context.SaveChangesAsync();

            return id;
        }

        public async Task<Branches> GetBranch(int branchId)
        {
            var branches = await context.Branches.FindAsync(branchId);
            return branches;
        }

    }
}
