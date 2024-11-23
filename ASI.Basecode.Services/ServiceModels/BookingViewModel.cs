using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BookingViewModel
    {
        public BookingViewModel()
        {
            //DayOfTheWeekIds = new List<int>();
        }
        public int BookingId { get; set; }
        public int? UserId { get; set; }
        public int? FacilityId { get; set; }
        public string BookingStatus { get; set; }
        public string Notes { get; set; }
        public string FacilityName { get; set; }

        public DateTime? BookingDate { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime? StartDate { get; set; }


        [Required(ErrorMessage = "End Date is required.")]
        public DateTime? EndDate { get; set; }

        public TimeSpan? BookingTimeStart { get; set; }
        public TimeSpan? BookingTimeEnd { get; set; }
        public DateTime? DtCreated { get; set; }
        public DateTime? DtUpdated { get; set; }
        //public string RoomName { get; set; }
        //public List<Recurrence> Recurrence { get; set; }
        public string UserName { get; set; }
        public User modelUser { get; set; }
        //public Facility modelRoom { get; set; }

        //public bool BookingChangeOnly { get; set; }
        //public List<DayOfTheWeek> Days { get; set; }
        //public List<int> DayOfTheWeekIds { get; set; }
        //public IEnumerable<BookingViewModel> bookingList { get; set; }
        public IEnumerable<FacilityViewModel> facilityList { get; set; }
    }
}
