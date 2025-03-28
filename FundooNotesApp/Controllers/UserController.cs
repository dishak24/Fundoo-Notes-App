using CommonLayer.Models;
using ManagerLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;


//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using System;
using System.Threading.Tasks;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;

        private readonly IBus bus;

        public UserController(IUserManager userManager, IBus bus)
        {
            this.userManager = userManager;
            this.bus = bus;
        }

        //register api
        //  api/user/Register
        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterModel model)
        {
            //adding code to checking email already used or not
            var check = userManager.CheckEmailExist(model.Email);
            if (check)
            {
                return BadRequest(new ResponseModel<UserEntity>
                {
                    Success = false,
                    Message = "Email already exist! Please, Enter another EmailId."
                });
            }
            else
            {
                var result = userManager.Register(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<UserEntity>
                    {
                        Success = true,
                        Message = "Registration Successfull !",
                        Data = result
                    });
                }
                return BadRequest(new ResponseModel<UserEntity>
                {
                    Success = false,
                    Message = "Registration failed !!!!",
                    Data = result
                });
            }
            
        }

        //login api
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginModel loginModel)
        {
            var result = userManager.Login(loginModel);
            if (result != null)
            {
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Login Successfull !",
                    Data = result

                });
            }
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = "Login failed !!!!",
                Data = result

            });

        }

        //Forgot password API
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                if (userManager.CheckEmailExist(email))
                {
                    Send send = new Send();
                    ForgotPasswordModel forgotPasswordModel = userManager.ForgotPassword(email);
                    send.SendingMail(forgotPasswordModel.Email, forgotPasswordModel.Token);
                    Uri uri = new Uri("rabbitmq://localhost/FunDooNotes_EmailQueue");
                    var endPoint = await bus.GetSendEndpoint(uri);

                    await endPoint.Send(forgotPasswordModel);

                    return Ok(new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Main sent Successfull !"

                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Sending email failed !!!!!"

                    });
                }

            }
            catch(Exception e)
            {
                throw e;
            }
        }

        //Reset Password API
        [Authorize]
        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            try
            {
                string email = User.FindFirst("EmailId").Value;
                if (userManager.ResetPassword(email, model))
                {
                    return Ok(new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Done, Password is Reset !"

                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Reseting Password Failed !!!!!"

                    });
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
