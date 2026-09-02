using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.PublicMassage
{
    public class MessageDTO
    {
        public int MessageID { get; set; }
        public int SenderID { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string SentTimeAndDate { get; set; } = string.Empty;
    }
}
