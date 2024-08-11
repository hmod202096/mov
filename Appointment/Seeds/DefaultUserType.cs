using Appointment.Data;
using Appointment.Enums;
using Appointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Seeds
{
    public static class DefaultUserType
    {
        public static async Task SeedUserTypeAsync(ApplicationDbContext context)
        {
           
            var model = new List<string>();
            model.Add(TypeUserEnum.Manager.ToString());
            model.Add(TypeUserEnum.Supervisor.ToString());
            model.Add(TypeUserEnum.Emplooye.ToString());
            model.Add(TypeUserEnum.Driver.ToString());
            model.Add(TypeUserEnum.Customer.ToString());
            foreach (var item in model)
            {
                UserTypes userTypes = new UserTypes()
                {
                    Type = item
                };

                var check = context.UserTypes.Where(m => m.Type == item).Count();
                if (check == 0)
                {
                    await context.UserTypes.AddAsync(userTypes);
                }

            }
            await context.SaveChangesAsync();
            // }
        }
    }
}
