using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.PublicUser
{
    public static class UserMapper
    {
        public static UserDTO MapToDTO(User _user)
        {
            return new UserDTO
            {
                UserName= _user.UserName,
                ID=_user.Id
            };

        }
    }
    
}
