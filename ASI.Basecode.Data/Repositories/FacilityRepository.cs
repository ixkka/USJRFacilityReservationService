using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class FacilityRepository : BaseRepository, IFacilityRepository
    {
        public FacilityRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public IQueryable<Facility> GetFacility()
        {
            return this.GetDbSet<Facility>();
        }
        public IQueryable<ImageGallery> GetFacilityGalleries()
        {
            return this.GetDbSet<ImageGallery>();
        }
        public bool FacilityExists(int facilityId)
        {
            return this.GetDbSet<Facility>().Any(x => x.FacilityId == facilityId);
        }   
        public void AddFacility(Facility facility)
        {
            this.GetDbSet<Facility>().Add(facility);
            UnitOfWork.SaveChanges();
        }
        public void AddFacility2(Facility facility)
        {
            this.GetDbSet<Facility>().Add(facility);
            UnitOfWork.SaveChanges();
        }
        public void UpdateFacility(Facility facility)
        {
            this.GetDbSet<Facility>().Update(facility);
            UnitOfWork.SaveChanges();
        }
        public void UpdateGallery(ImageGallery imageGallery)
        {
            this.GetDbSet<ImageGallery>().Update(imageGallery);
            UnitOfWork.SaveChanges();
        }

        public void DeleteFacility(Facility facility)
        {
            this.GetDbSet<Facility>().Remove(facility);
            UnitOfWork.SaveChanges();
        }
        public void DeleteFacilityImage(ImageGallery imageGallery)
        {
            this.GetDbSet<ImageGallery>().Remove(imageGallery);
            UnitOfWork.SaveChanges();
        }

        public IQueryable<DayOfTheWeek> GetDays()
        {
            return this.GetDbSet<DayOfTheWeek>();
        }
    }
}