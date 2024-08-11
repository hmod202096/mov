using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Active = true;
        }

        [Required]
        public string Name { get; set; }
        public string City { get; set; }
        public bool Active { get; set; }
        public int UserTypesId { get; set; }
        [ForeignKey("UserTypesId")]
        public virtual UserTypes UserTypes { get; set; }

        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branches Branch { get; set; }

    }
}
