using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;

        public FacilityService(IFacilityRepository facilityRepository, IMapper mapper)
        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }

        public IEnumerable<FacilityViewModel> RetrieveAll(string facilityName = null)
        {
            var data = _facilityRepository.GetFacility()
                .Where(x => (string.IsNullOrEmpty(facilityName) || x.FacilityName.Contains(facilityName))).Select(s => new FacilityViewModel
                {
                    FacilityId = s.FacilityId,
                    FacilityName = s.FacilityName,
                    Description = s.Description,
                    Location = s.Location,
                    Capacity = s.Capacity.Value,
                    Thumbnail = s.Thumbnail,
                    Amenity = s.Amenity,
                    _RoomGallery = s.ImageGalleries.Select(i => new RoomGalleryViewModel
                    {
                        FacilityId = s.FacilityId,
                        GalleryId = i.ImageId,
                        GalleryName = i.ImageName,
                        GalleryUrl = i.Path,

                    }).ToList(),
                });
            return data;
        }

        public void AddFacility(FacilityViewModel model)
        {
            var newModel = new Facility();
            _mapper.Map(model, newModel);
            newModel.CreatedDt = DateTime.Now;

            newModel.ImageGalleries = new List<ImageGallery>();

            if (model._RoomGallery != null && model._RoomGallery.Any())
            {
                foreach (var file in model._RoomGallery)
                {
                    newModel.ImageGalleries.Add(new ImageGallery()
                    {
                        ImageName = file.GalleryName,
                        Path = file.GalleryUrl,
                    });
                }
            }

            _facilityRepository.AddFacility(newModel);
        }
        public void AddFacility2(FacilityViewModel model)
        {
            var facility = new Facility();
            _mapper.Map(model, facility);
            facility.FacilityId = model.FacilityId;
            facility.FacilityName = model.FacilityName;
            facility.Description = model.Description;
            facility.Location = model.Location;
            facility.Capacity = model.Capacity;
            facility.Amenity = model.Amenity;
            facility.Thumbnail = model.Thumbnail;
            facility.BookingDays = model.BookingDays;
            facility.BookingDuration = model.BookingDuration;
            facility.BookingHoursStart = model.BookingHoursStart;
            facility.BookingHoursEnd = model.BookingHoursEnd;
            facility.BookingPrice = model.BookingPrice;
            facility.CreatedBy = Environment.UserName;
            facility.UpdatedBy = Environment.UserName;

            _facilityRepository.AddFacility2(facility);
        }

        public void UpdateFacility(FacilityViewModel model)
        {
            var existingData = _facilityRepository.GetFacility().Where(s => s.FacilityId == model.FacilityId).FirstOrDefault();
            _mapper.Map(model, existingData);
            existingData.UpdatedDt = DateTime.Now;
            existingData.Thumbnail = model.Thumbnail;
            existingData.UpdatedDt = DateTime.Now;
            existingData.UpdatedBy = System.Environment.UserName;

            if (model._RoomGallery != null && model._RoomGallery.Any())
            {
                foreach (var file in model._RoomGallery)
                {
                    existingData.ImageGalleries.Add(new ImageGallery()
                    {
                        ImageName = file.GalleryName,
                        Path = file.GalleryUrl,
                    });
                }
            }

            _facilityRepository.UpdateFacility(existingData);
        }

        public void UpdateGallery(RoomGalleryViewModel model)
        {
            var existingData = _facilityRepository.GetFacilityGalleries().Where(s => s.FacilityId == model.FacilityId).ToList();

            if (existingData != null && existingData.Any())
            {
                foreach (var item in existingData)
                {
                    _facilityRepository.UpdateGallery(item);
                }
            }
        }

        public void DeleteFacility(FacilityViewModel room)
        {
            var RoomToBeDeleted = _facilityRepository.GetFacility().Where(u => u.FacilityId == room.FacilityId).FirstOrDefault();
            if (RoomToBeDeleted != null)
            {
                _facilityRepository.DeleteFacility(RoomToBeDeleted);
            }
        }

        public IEnumerable<RoomGalleryViewModel> GetRoomGallery()
        {
            var data = _facilityRepository.GetFacilityGalleries().Select(s => new RoomGalleryViewModel
            {
                FacilityId = (int)s.FacilityId,
                GalleryId = s.ImageId,
                GalleryUrl = s.Path,
                GalleryName = s.ImageName,
            });
            return data;
        }
        public void DeleteImage(RoomGalleryViewModel model)
        {
            var roomImages = _facilityRepository.GetFacilityGalleries().Where(x => x.FacilityId == model.FacilityId).ToList();
            if (roomImages != null && roomImages.Any())
            {
                foreach (var item in roomImages)
                {
                    _facilityRepository.DeleteFacilityImage(item);
                }
            }
        }

        public IEnumerable<FacilityViewModel> GetFacilities()
        {
            var facilities = _facilityRepository.GetFacility().ToList();
            return facilities.Select(f => new FacilityViewModel
            {
                FacilityId = f.FacilityId,
                FacilityName = f.FacilityName,
                Description = f.Description,
                Location = f.Location,
                Capacity = f.Capacity ?? 0,
                Thumbnail = f.Thumbnail,
                Amenity = f.Amenity,
                BookingDays = f.BookingDays,
                BookingHoursStart = f.BookingHoursStart,
                BookingHoursEnd = f.BookingHoursEnd,
                BookingDuration = f.BookingDuration,
                BookingPrice = f.BookingPrice,
            }).ToList();
        }

        public IEnumerable<DayOfTheWeek> GetDays()
        {
            return _facilityRepository.GetDays();
        }
    }
}
