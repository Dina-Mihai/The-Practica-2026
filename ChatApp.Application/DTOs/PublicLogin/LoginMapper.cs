using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.PublicLogin
{
    public static class LoginMapper
    {
        public static LoginDTO MapToDTO_Login(User user)
        {
            return new LoginDTO
            {
                LoginPassword = user.Password,
                LoginEmail = user.Email,
            };
        }

        public static object MapToDTO_Login(LoginDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
