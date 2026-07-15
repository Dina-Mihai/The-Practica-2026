using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities
{//trebuie sa las internal sau public

    public class Message
    {
        //daca nu scriu public imi face automat privat?
        DateOnly SentDate;
        TimeOnly SentTime;
        string SenderID;
        string RiceiverID;


    }
    //DTO trebuie facut in acelasi fiser cu clasa?+nu inteleg exact cum se face
    public class MessageDTO
    {
        DateOnly SentDate;
        TimeOnly SentTime;
        string SenderID;
        string RiceiverID;
    }

}
/* continut
 * id
 * ora timiteri
 * user
 * catre cine
 */ 