using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepo userRepo;

        public UserManager(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public UserEntity Register(RegisterModel model)
        {
            return userRepo.Register(model);
        }

        public bool CheckEmailExist(string email)
        {
            return userRepo.CheckEmailExist(email);
        }

        public string Login(LoginModel loginModel)
        {
            return userRepo.Login(loginModel);
        }

        public ForgotPasswordModel ForgotPassword(string email)
        {
            return userRepo.ForgotPassword(email);
        }

        public bool ResetPassword(string email, ResetPasswordModel reset)
        {
            return userRepo.ResetPassword(email, reset);
        }
    }
}
