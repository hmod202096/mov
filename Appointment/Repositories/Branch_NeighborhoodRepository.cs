using Appointment.Data;
using Appointment.Models;
using Appointment.Models.ViewModel;
using Appointment.Service;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public class Branch_NeighborhoodRepository : IBranch_NeighborhoodRepository
    {
        private readonly ApplicationDbContext context;

        public Branch_NeighborhoodRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Branch_NeighborhoodViewModel>> GetNeighborhood(int branchId)
        {
            var data = await context.Neighborhoods.OrderBy(m => m.Name).ToListAsync();

            var model = new List<Branch_NeighborhoodViewModel>();

            foreach (var item in data)
            {
                var branch_NeighborhoodModel = new Branch_NeighborhoodViewModel
                {
                    NeighborhoodId = item.Id,
                    NeighborhoodName = item.Name,
                    IsSelected = await GetBranchIdNeighborhoodId(branchId, item.Id)
                };

                model.Add(branch_NeighborhoodModel);
            }

            return model;
        }

        public async Task<bool> GetBranchIdNeighborhoodId(int branchId, int neighborhoodId)
        {
            var data = await context.Branches_Neighborhoodes.Where(x => x.BranchId == branchId && x.NeighborhoodId == neighborhoodId).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }


        public async Task EditNeighborhoodInBranch(int branchId, List<Branch_NeighborhoodViewModel> model)
        {
            for (int i = 0; i < model.Count; i++)
            {
                var neighborhoodId = model[i].NeighborhoodId;

                if (model[i].IsSelected && await GetBranchIdNeighborhoodId(branchId, neighborhoodId) == false)
                {
                    Branches_Neighborhoodes tb = new Branches_Neighborhoodes
                    {
                        BranchId = branchId,
                        NeighborhoodId = model[i].NeighborhoodId,
                    };

                    await context.Branches_Neighborhoodes.AddAsync(tb);
                }
                else if (!model[i].IsSelected && await GetBranchIdNeighborhoodId(branchId, neighborhoodId) == true)
                {
                    var movie = await context.Branches_Neighborhoodes.Where(x => x.BranchId == branchId && x.NeighborhoodId == model[i].NeighborhoodId).FirstOrDefaultAsync();
                    context.Branches_Neighborhoodes.Remove(movie);
                }
            }
            await context.SaveChangesAsync();
        }

        public async Task<List<SelectListItem>> GetNeighborhoodByBranchId(int branchId)
        {
            var data = await context.Branches_Neighborhoodes
                .Include(p => p.Branches)
                .Include(p => p.Neighborhoods)
                .Where(p => p.BranchId == branchId)
                .Select(p => new Branches_Neighborhoodes
                {
                    NeighborhoodId = p.NeighborhoodId,
                    Neighborhoods = p.Neighborhoods
                })
                .ToListAsync();

            var model = new List<SelectListItem>();

            foreach (var item in data)
            {
                model.Add(new SelectListItem()
                {
                    Text = item.Neighborhoods.Name,
                    Value = item.NeighborhoodId.ToString()
                });
            }

            return model;

        }
    }
}
