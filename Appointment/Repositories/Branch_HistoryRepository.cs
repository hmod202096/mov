using Appointment.Data;
using Appointment.Models;
using Appointment.Models.ViewModel;
using Appointment.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public class Branch_HistoryRepository : IBranch_HistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public Branch_HistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Branch_HistoryDateViewModel>> GetAll(int branchId)
        {
            var branch = await _context.Branches.FindAsync(branchId);
            int maxDate = branch.MaxDate;

            return await _context.Branches_HistoryDates
                .Where(x => x.BranchId == branchId && x.HistoryDate.Date.Date >= DateTime.Now.Date)
                .OrderBy(x => x.HistoryDate.Date)
                .Select(b => new Branch_HistoryDateViewModel()
                {
                    HistoryDateId = b.HistoryDateId,
                    BranchId = branchId,
                    GregorianDate = b.HistoryDate.Date.ToString("dd/MM/yyyy"),
                    IslamicHistory = b.HistoryDate.Date.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("ar-SA")),
                    Day = b.HistoryDate.Date.ToString("dddd", new System.Globalization.CultureInfo("ar-SA")),
                    CountBooking = b.CountBooking,
                }).Take(maxDate).ToListAsync();
        }


        public async Task<List<Branch_HistoryDateViewModel>> Edit(List<Branch_HistoryDateViewModel> branch_HistoriesList)
        {
            //var data = new List<Branch_HistoriesModel>();

            for (int i = 0; i < branch_HistoriesList.Count; i++)
            {
                Branches_HistoryDates branches_HistoryDates = new Branches_HistoryDates
                {
                    BranchId = branch_HistoriesList[i].BranchId,
                    HistoryDateId = branch_HistoriesList[i].HistoryDateId,
                    CountBooking = branch_HistoriesList[i].CountBooking
                };

                _context.Branches_HistoryDates.Update(branches_HistoryDates);
            }

            await _context.SaveChangesAsync();

            return branch_HistoriesList;
        }

    }
}
