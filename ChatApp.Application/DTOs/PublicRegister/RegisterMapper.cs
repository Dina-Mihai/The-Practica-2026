using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.PublicRegister
{
    public static class RegisterMapper
    {
        public static RegisterDTO MapToDot_Register(User RegisterUser)
        {
            return new RegisterDTO
            {
                RegisterEmail = RegisterUser.Email,
                RegisterName = RegisterUser.UserName,
                RegisterPassword = RegisterUser.Password,
            };
        }

        public static object MapToDot_Register(RegisterDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
