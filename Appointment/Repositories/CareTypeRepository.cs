using Appointment.Data;
using Appointment.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public class CareTypeRepository : ICareTypeRepository
    {
        private readonly ApplicationDbContext context;

        public CareTypeRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<CareTypes>> GetAll()
        {
            var model = await context.CareTypes.OrderBy(m => m.Name).ToListAsync();

            return model;
        }

        public async Task<int> Add(CareTypes model)
        {
            context.CareTypes.Add(model);
            await context.SaveChangesAsync();
            return model.Id;
        }

        public async Task<CareTypes> Find(int? id)
        {
            var model = await context.CareTypes.FindAsync(id);
            return model;
        }

        public async Task<CareTypes> Edit(CareTypes model)
        {
            context.CareTypes.Update(model);
            await context.SaveChangesAsync();
            return model;
        }

        public async Task<CareTypes> Delete(CareTypes model)
        {
            context.CareTypes.Remove(model);
            await context.SaveChangesAsync();
            return model;
        }
    }
}
