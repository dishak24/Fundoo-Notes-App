using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class ForgotPasswordModel
    {
        //for returning data,  not for input/request
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
