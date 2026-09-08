using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace ChatApp.Domain.Entities
{
    public class Conversation
    {


        public int ID { get; set; }
        public DateTime SentTimeAndDate { get; set; }
        public string ConversationName { get; set; }
        public List<Message>Messages { get; set; }
        public List<ConversationParticipant> ConversationParticipants { get; set; }

        public Conversation(int id, DateTime Sent_time, List<Message> the_message, List<ConversationParticipant> The_users ) 
        {
            ID = id;
            SentTimeAndDate = Sent_time;
            ConversationName = "Conversation" + id;
            Messages = the_message;
            ConversationParticipants = The_users;

        }
        public Conversation() { }
    }

}
