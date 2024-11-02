using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IFacilityRepository
    {
        IQueryable<Facility> GetFacility();
        IQueryable<ImageGallery> GetFacilityGalleries();
        bool FacilityExists(int facilityId);
        void AddFacility(Facility facility);
        void UpdateFacility(Facility facility);
        void UpdateGallery(ImageGallery imageGallery);
        void DeleteFacility(Facility room);
        void DeleteFacilityImage(ImageGallery imageGallery);

        IQueryable<DayOfTheWeek> GetDays();
    }
}