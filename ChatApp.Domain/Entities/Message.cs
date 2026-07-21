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
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }

        public Message(DateOnly _sent_date , TimeOnly _sent_time, string _sender_id, string _riciver_id)
        {
            SentDate = _sent_date;
            SentTime = _sent_time;
            SenderID = _sender_id;
            ReceiverID = _riciver_id;
        }


    }

}
