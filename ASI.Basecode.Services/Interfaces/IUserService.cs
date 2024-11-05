using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IUserService
    {
        LoginResult AuthenticateUser(string userid, string password, ref User user);
        void AddUser(UserViewModel model);
        List<UserViewModel> GetAllUsers();
     
        bool DeleteUserById(int id); // Declared as bool in the interface

        /*  UserViewModel GetUserById(int id);*/


        User GetUserById(string userId); // Change to accept string
        void UpdateUser(User user);



    }

}
