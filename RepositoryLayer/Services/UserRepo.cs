using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;

        public UserRepo(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        //For register user
        public UserEntity Register(RegisterModel registerModel)
        {
            UserEntity user = new UserEntity();
            user.FirstName = registerModel.FirstName;
            user.LastName = registerModel.LastName;
            user.DOB = registerModel.DOB;
            user.Gender = registerModel.Gender;
            user.Email = registerModel.Email;
            user.Password = EncodePasswordToBase64(registerModel.Password);//encrypt password

            this.context.Users.Add(user);
            context.SaveChanges();
            return user;
        }


        //To encrypt Password for security.
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encryption_Data = new byte[password.Length];
                encryption_Data = System.Text.Encoding.UTF8.GetBytes(password);
                string encoded_Data = Convert.ToBase64String(encryption_Data);
                return encoded_Data;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64 encode " + e.Message);
            }
        }

        //checking email already exit or not
        public bool CheckEmailExist(string email)
        {
            var result = this.context.Users.FirstOrDefault(x => x.Email == email);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        //for login user
        public string Login(LoginModel loginModel)
        {
            var checkUser = this.context.Users.FirstOrDefault( u => u.Email == loginModel.Email && u.Password == EncodePasswordToBase64(loginModel.Password));
            if (checkUser != null )
            {
                //return user;
                var token = GenerateToken(checkUser.Email, checkUser.UserId);
                return token;
            }
            return null;            
        }


        // To generate token
        private string GenerateToken(string emailId, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("EmailId", emailId),
                new Claim("UserId", userId.ToString())
            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        //Forgot password method.
        public ForgotPasswordModel ForgotPassword(string email)
        {
            var User = context.Users.ToList().Find( u => u.Email == email );
            ForgotPasswordModel forgotPassword = new ForgotPasswordModel();
            forgotPassword.UserId = User.UserId;
            forgotPassword.Email = User.Email;
            forgotPassword.Token = GenerateToken(User.Email, User.UserId);
            return forgotPassword;
        }

        //To Reset the password:
        //1. check email exist or not
        //2. then change password
        public bool ResetPassword(string email, ResetPasswordModel reset)
        {
            var user = context.Users.ToList().Find(u => u.Email == email);

            if (CheckEmailExist(user.Email))
            {
                user.Password = EncodePasswordToBase64(reset.ConfirmPassword);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
