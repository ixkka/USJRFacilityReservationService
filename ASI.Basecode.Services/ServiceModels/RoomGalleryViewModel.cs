using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class RoomGalleryViewModel
    {
        public int FacilityId { get; set; }
        public int GalleryId { get; set; }
        public string GalleryName { get; set; }
        public string GalleryUrl { get; set; }
    }
}
