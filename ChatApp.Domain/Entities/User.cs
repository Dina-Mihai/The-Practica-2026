using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class User
    {
        public string UserName;
        public string Password;
        public string Email;
        public int  Id;
        public User(string userName, string password, string email, int id)
        {
            UserName = userName;
            Password = password;
            Email = email;
            Id = id;
        }
    }
}


/*clase: user:-nume
 *            -parola
 *            -adresa de mail
 *            -user name
 *            -#poza
 *            -id
 *            -#data de creare
 */
//nu am acces la fiserul README.mb in visualstudio