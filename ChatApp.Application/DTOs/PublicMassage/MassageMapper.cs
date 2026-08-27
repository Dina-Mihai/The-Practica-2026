using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChatApp.Application.DTOs.PublicMassage
{
    public static class MassageMapper
    {
        public static MessageDTO MapToDTO_Message(Message message)
        {
            return new MessageDTO
            {
                SentTimeAndDate = $"{message.SentDate} {message.SentTime}",
                SenderID = message.SenderID

            };
        }
    }
}
