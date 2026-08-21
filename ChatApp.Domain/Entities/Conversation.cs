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
        public Message SentTimeAndDate { get; set; }
        public ConversationParticipant ConversationID { get; set; }
        public List<Message>Messages { get; set; }
        public ConversationParticipant ConversationParticipants { get; set; }

        public Conversation(int id, ConversationParticipant Conversation_id, Message Sent_time, List<Message> the_message, ConversationParticipant The_users ) 
        {
            ID = id;
            SentTimeAndDate = Sent_time;
            ConversationID = Conversation_id;
            Messages = the_message;
            ConversationParticipants = The_users;

        }
    }

}
