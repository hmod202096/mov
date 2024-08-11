using Appointment.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public interface ICustomerRepository
    {
        Task<CustomerViewModel> SearchByMobily(string term, int branchId, bool sav);
        Task<int> EditCustomer(CustomerViewModel model);
    }
}
