using Appointment.Data;
using Appointment.Models;
using Appointment.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<CustomerViewModel> SearchByMobily(string term, int branchId, bool sav)
        {
            if (!string.IsNullOrEmpty(term))
            {
                var customer = await _context.Customers
                              .Include(x => x.Neighborhoods)
                              .Where(m => m.Mobily == term).ToListAsync();

                if (customer.Count > 0)
                {
                    CustomerViewModel customerViewModel = new CustomerViewModel
                    {
                        Id = customer.FirstOrDefault().Id,
                        Name = customer.FirstOrDefault().Name,
                        Mobily = customer.FirstOrDefault().Mobily,
                        NeighId = customer.FirstOrDefault().Neighborhoods.Id,
                        NeighborhoodsList = await GetNeighborhoodsAsync(branchId),
                    };

                    return customerViewModel;
                }
            }

            if (sav == true)
            {
                CustomerViewModel model = new CustomerViewModel
                {

                    NeighborhoodsList = await GetNeighborhoodsAsync(branchId),
                    StatusMessage = "success : تمت عملية التعديل بنجاح "
                };

                return model;
            }

            return null;
           
        }

        //Get List Neighborhood
        public async Task<List<Neighborhoods>> GetNeighborhoodsAsync(int branchId)
        {
            var neigh = await _context.Branches_Neighborhoodes.Include(m => m.Neighborhoods).Where(m => m.BranchId == branchId).ToListAsync();

            var newNeigh = new List<Models.Neighborhoods>();

            foreach (var item in neigh)
            {
                newNeigh.Add(new Models.Neighborhoods { Id = item.NeighborhoodId, Name = item.Neighborhoods.Name });
            }

            return newNeigh;
        }

        public async Task<int> EditCustomer(CustomerViewModel model)
        {
            var customer = await _context.Customers.FindAsync(model.Id);

            customer.Id = model.Id;
            customer.Name = model.Name;
            customer.Mobily = model.Mobily;
            customer.NeighId = model.NeighId;

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return customer.Id;
        }

    }
}
