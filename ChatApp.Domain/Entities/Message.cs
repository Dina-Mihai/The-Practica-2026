using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{

    public class Message
    {
      
        public DateOnly SentDate { get; set; }
        public TimeOnly SentTime { get; set; }
        public int SenderID { get; set; }
        public User Sender { get; set; }
        public int MessageID { get; set; }
        public string Content { get; set; }
        public int ConversationID { get; set; }
        public Conversation Conversation { get; set; }

        public Message(DateOnly Sent_date , TimeOnly Sent_time, int Sender_id, User Sender_name, int Message_id, string The_content, int Content_id, Conversation The_conversation)
        {
            SentDate = Sent_date;
            SentTime = Sent_time;
            SenderID = Sender_id;  
            Sender = Sender_name;
            MessageID = Message_id;
            Content = The_content;
            Conversation = The_conversation;
            

        }
        public Message()
        {

        }


    }

}
