using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepo
    {
        //for register user
        public UserEntity Register(RegisterModel registerModel);

        //for duplicate email checking
        public bool CheckEmailExist(string email);

        //for login user
        public string Login(LoginModel loginModel);

        //Forgot password
        public ForgotPasswordModel ForgotPassword(string email);

        //Reset passwod
        public bool ResetPassword(string email, ResetPasswordModel reset);
    }
}
