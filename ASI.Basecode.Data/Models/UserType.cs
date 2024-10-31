using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Models
{
    public class UserType
    {
       
            public int UserTypeId { get; set; }
            public string UserTypeName { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
