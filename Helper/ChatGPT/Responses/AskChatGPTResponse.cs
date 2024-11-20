using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.ChatGPT.Responses
{
    public class AskChatGPTResponse
    {
        public string text { get; set; }
        public string finish_reason { get; set; }
        public string model { get;set; }
        public string server { get; set; }
    }
}
