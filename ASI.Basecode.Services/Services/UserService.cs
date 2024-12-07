using ASI.Basecode.Data;
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
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public LoginResult AuthenticateUser(string userId, string password, ref User user)
        {
            user = new User();
            var passwordKey = PasswordManager.EncryptPassword(password);
            user = _repository.GetUsers().FirstOrDefault(x => x.UserId == userId && x.Password == passwordKey);

            return user != null ? LoginResult.Success : LoginResult.Failed;
        }

        public void AddUser(UserViewModel model)
        {
            var user = new User();

            if (!_repository.UserExists(model.UserId))
            {
                // Map UserViewModel to User entity
                _mapper.Map(model, user);

                // Encrypt the password and set other properties
                user.Password = PasswordManager.EncryptPassword(model.Password);
                user.CreatedTime = DateTime.Now;
                user.UpdatedTime = DateTime.Now;
                user.CreatedBy = Environment.UserName;
                user.UpdatedBy = Environment.UserName;

                // Save the user using the repository
                _repository.AddUser(user);
            }
            else
            {
                throw new InvalidDataException("User already exists.");
            }
        }



        public List<UserViewModel> GetAllUsers()
        {
            var users = _repository.GetUsers().ToList();
            return _mapper.Map<List<UserViewModel>>(users);
        }

        public bool DeleteUserById(int id)
        {
            try
            {
                return _repository.DeleteUserById(id);
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return false;
            }
        }

        // Implement GetUserById
        public User GetUserById(string userId)
        {
            return _repository.GetUserById(userId); // Adjust repository method accordingly
        }

        public void UpdateUser(User user)
        {
            _repository.UpdateUser(user);
        }


        public void AddUserAdmin(UserViewModel model)
        {
            // Map UserViewModel to User entity
            var user = new User
            {
                UserId = model.UserId,
                Name = model.Name,
                Password = PasswordManager.EncryptPassword(model.Password), // Encrypt password
                Department = model.Department,
                CreatedTime = DateTime.Now,
                CreatedBy = "Admin", // Admin-specific addition
                                    // Add other fields if needed
                UserTypeId = model.UserTypeId
            };

            // Save the user to the database
            _repository.AddUser(user);
        }
    }
}
