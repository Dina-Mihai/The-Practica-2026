using ChatApp.Application.DTOs.PublicRegister;
using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChatApp.Application.DTOs.PublicUser
{
    public static class UserMapper
    {
        public static UserDTO MapToDTO_User(User user)
        {
            return new UserDTO
            {
                UserName = user.UserName,
                UserID = user.Id,
                Email = user.Email,
                Password = user.Password,
            };

        }

      
    }
    
}
