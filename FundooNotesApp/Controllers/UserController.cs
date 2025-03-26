using CommonLayer.Models;
using ManagerLayer.Interfaces;
//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;

        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterModel model)
        {
            //adding code to checking email already used or not
            var check = userManager.CheckEmailDuplicate(model.Email);
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
    }
}
