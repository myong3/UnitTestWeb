using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestWeb.Models
{
    public class MessageModel
    {
        public MessageModel()
        {
            responseCode = "0";
            errMsg = "";
        }

        public void SetMessage(MessageModel message)
        {
            responseCode = message.responseCode;
            errMsg = message.errMsg;
        }

        public string responseCode { set; get; }
        public string errMsg { set; get; }
    }
}