using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class Reservations
    {
        public Reservations()
        {
            IsDelete = false;
        }

        private DateTime _releaseDate = DateTime.MinValue;

        public int Id { get; set; }

        [Display(Name = "التاريخ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AppointmentDate
        {
            get
            {
                return (_releaseDate == DateTime.MinValue) ? DateTime.Now : _releaseDate;
            }
            set
            {
                _releaseDate = value;
            }
        }

        [Required(ErrorMessage = "يجب اختيار الفترة")]
        [Display(Name = "الفترة")]
        public string Priod { get; set; }

        public enum Epriod { ص = 0, م = 1 }
        [Required(ErrorMessage = "يجب ادخال التبرعات")]
        [Display(Name ="التبرعات")]
        public string Donation { get; set; }

        [Display(Name = "الملاحظات")]
        public string Comments { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public bool IsDelete { get; set; }
        public string CommentsDelete { get; set; }

        public string Status { get; set; }

        public string Notes { get; set; }

        public string DriverId { get; set; }
        [ForeignKey("DriverId")]
        public virtual ApplicationUser ApplicationUserDriver { get; set; }

        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branches Branches { get; set; }

        public int CustId { get; set; }
        [ForeignKey("CustId")]
        public virtual Customers Customers { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
