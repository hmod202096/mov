using Appointment.Data;
using Appointment.Models;
using Appointment.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public class NeighborhoodRepository : INeighborhoodRepository
    {
        private readonly ApplicationDbContext context;

        public NeighborhoodRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        [TempData]
        public string statusMessage { get; set; }

        public async Task<NeighborhoodViewModel> GetAll(int pageNumber, string searchName = null)
        {

            StringBuilder param = new StringBuilder();

            param.Append("/Admin/Neighborhoods/Index?pageNumber=:");

            param.Append("&searchName");

            if (searchName != null)
            {
                param.Append(searchName);
            }
            else
            {
                searchName = "";
            }


            NeighborhoodViewModel neighborhoodViewModel = new NeighborhoodViewModel()
            {
                Neighborhoods = await context.Neighborhoods.OrderBy(m => m.Name).Where(m => m.Name.Contains(searchName)).ToListAsync(),
                Paginginfo = new Paginginfo()

            };

            var count = neighborhoodViewModel.Neighborhoods.Count();
            int pageSize = 10;

            neighborhoodViewModel.Neighborhoods = neighborhoodViewModel.Neighborhoods.OrderBy(o => o.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            neighborhoodViewModel.Paginginfo = new Paginginfo()
            {
                CurrentPage = pageNumber,
                RecordsPerPage = pageSize,
                TotalRecords = count,
                UrlParam = param.ToString()
            };

            return neighborhoodViewModel;

        }

        public async Task<NeighborhoodOperationViewModel> AddNeighborhood(NeighborhoodOperationViewModel neighborhoodCreateViewModel)
        {

            var doseExistNeighborhood = await context.Neighborhoods.Where(m => m.Name.Equals(neighborhoodCreateViewModel.Neighborhoods.Name)).ToListAsync();

            if (doseExistNeighborhood.Count() > 0)
            {
                statusMessage = "Error : اسم الحي " + doseExistNeighborhood.FirstOrDefault().Name + " مسجل مسبقاً";

            }
            else
            {
                context.Neighborhoods.Add(neighborhoodCreateViewModel.Neighborhoods);
                await context.SaveChangesAsync();

                return neighborhoodCreateViewModel;
            }

            neighborhoodCreateViewModel.StatusMessage = statusMessage;

            return neighborhoodCreateViewModel;
        }

        public async Task<NeighborhoodOperationViewModel> GetNeighborhood(int? id)
        {
            var neigh = await context.Neighborhoods.FindAsync(id);

            NeighborhoodOperationViewModel model = new NeighborhoodOperationViewModel
            {
                Neighborhoods = neigh
            };

            return model;
        }

        public async Task<NeighborhoodOperationViewModel> EditNeighborhood(NeighborhoodOperationViewModel model)
        {

            var doseExistNeighborhood = await context.Neighborhoods.Where(m => m.Name.Equals(model.Neighborhoods.Name)).ToListAsync();

            if (doseExistNeighborhood.Count() > 0)
            {
                statusMessage = "Error : اسم الحي " + doseExistNeighborhood.FirstOrDefault().Name + " مسجل مسبقاً";

            }
            else
            {
                context.Neighborhoods.Update(model.Neighborhoods);
                await context.SaveChangesAsync();
                return model;
            }

            model.StatusMessage = statusMessage;

            return model;

        }

        public async Task<NeighborhoodOperationViewModel> DeleteNeighborhood(NeighborhoodOperationViewModel model)
        {
            context.Neighborhoods.Remove(model.Neighborhoods);
            await context.SaveChangesAsync();
            return model;
        }

    }
}
