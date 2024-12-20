using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Department { get; set; }

        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }

        public string ProfilePictureUrl { get; set; }
        public virtual ICollection<BookingPreference> BookingPreferences { get; set; }
    }
}
