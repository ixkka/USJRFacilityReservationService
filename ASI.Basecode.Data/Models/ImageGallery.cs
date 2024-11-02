using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public partial class ImageGallery
    {
        public int ImageId { get; set; }
        public int? FacilityId { get; set; }
        public string ImageName { get; set; }
        public string Path { get; set; }

        public virtual Facility Facility { get; set; }
    }
}
