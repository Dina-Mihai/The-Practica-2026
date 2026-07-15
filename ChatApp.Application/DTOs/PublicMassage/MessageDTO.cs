using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.PublicMassage
{
    public class MessageDTO
    {
        public string SentTimeAndDate { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
    }
}
