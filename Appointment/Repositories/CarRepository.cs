using Appointment.Data;
using Appointment.Models;
using Appointment.Models.ViewModel;
using Appointment.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext context;

        public CarRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        [TempData]
        public string statusMessage { get; set; }

        public async Task<List<Cares>> GetAll(int branchId)
        {
            var model = await context.Cares.Include(m => m.CareTypes).Where(m => m.Active == true && m.BranchId == branchId).ToListAsync();

            return model;
        }

        public async Task<CarAndCarTypeViewModel> CreateNew(int branchId)
        {
            CarAndCarTypeViewModel carAndCarTypeViewModel = new CarAndCarTypeViewModel
            {
                CareTypesList = await context.CareTypes.ToListAsync(),
                Cares = new Cares() 
                { 
                 BranchId = branchId
                },
                StatusMessage = statusMessage,
            };

            return carAndCarTypeViewModel;
        }

        public async Task<CarAndCarTypeViewModel> AddCar(CarAndCarTypeViewModel model)
        {
            var doseExistCar = await context.Cares.Where(m => m.PlatId.Equals(model.Cares.PlatId)).ToListAsync();

            if (doseExistCar.Count() > 0)
            {
                statusMessage = "Error : رقم اللوحة " + doseExistCar.FirstOrDefault().PlatId + " مسجل مسبقاً";

            }
            else
            {
                context.Cares.Add(model.Cares);
                await context.SaveChangesAsync();

                return model;
            }

            model.StatusMessage = statusMessage;
            return model;
        }

        public async Task<CarAndCarTypeViewModel> GetCar(string id)
        {
            var car = await context.Cares.FindAsync(id);

            CarAndCarTypeViewModel model = new CarAndCarTypeViewModel
            {
                Cares = car,
                CareTypesList = await context.CareTypes.ToListAsync(),
                StatusMessage = statusMessage
            };

            return model;
        }

        public async Task<CarAndCarTypeViewModel> EditCar(CarAndCarTypeViewModel model)
        {
                context.Cares.Update(model.Cares);
                await context.SaveChangesAsync();
                return model;
        }

        public async Task<Cares> DeleteCar(CarAndCarTypeViewModel model)
        {
            var car = await context.Cares.FindAsync(model.Cares.PlatId);
            car.Active = false;

            context.Cares.Update(car);
            await context.SaveChangesAsync();
            return car;
        }

    }
}
