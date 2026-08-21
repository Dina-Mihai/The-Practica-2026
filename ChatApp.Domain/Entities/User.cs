using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
        public List<Message> Message;
        public List<ConversationParticipant> ConversationParticipants;

        public User(string userName, string password, string email, int id, List<Message> themessages, List<ConversationParticipant> The_Participants)
        {
            UserName = userName;
            Password = password;
            Email = email;
            Id = id;
            Message = themessages;
            ConversationParticipants = The_Participants;
        }
    }
}
