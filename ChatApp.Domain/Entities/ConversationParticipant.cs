using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{
    public class ConversationParticipant
    {
        public int ConversationID { get; set; }// daca am conversatin ID nu imi mai trebuie ID simplu, nu?
        public Conversation Conversation { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
 

        public ConversationParticipant(int conversation_id, User user, int id, Conversation The_conversation)
        {
            ConversationID = conversation_id;
            User = user;
            UserID = id;
            Conversation = The_conversation;
        }
        public ConversationParticipant()
        {

        }

    }
}
