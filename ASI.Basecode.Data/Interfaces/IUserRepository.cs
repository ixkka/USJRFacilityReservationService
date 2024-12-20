﻿using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetUsers();
        bool UserExists(string userId);
        void AddUser(User user);

        bool DeleteUserById(int id);

        User GetUserById(string userId); // Accept string as parameter
        void UpdateUser(User user);

        void Add(User user);
    }
}
