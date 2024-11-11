using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IFacilityService
    {
        IEnumerable<FacilityViewModel> RetrieveAll(string roomName = null);
        void AddFacility(FacilityViewModel model);
        void AddFacility2(FacilityViewModel model);
        void UpdateFacility(FacilityViewModel model);
        void UpdateGallery(RoomGalleryViewModel model);
        void DeleteFacility(FacilityViewModel roomId);
        void DeleteImage(RoomGalleryViewModel model);
        IEnumerable<RoomGalleryViewModel> GetRoomGallery();
        IEnumerable<DayOfTheWeek> GetDays();
    }
}
