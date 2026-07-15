using System;

namespace ChatApp.Domain.Entities
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int  Id { get; set; }

        public User(string name, string password, string email, int id)
        {
            UserName = name;
            Password = password;
            Email = email;
            Id = id;
        }
    }

}

