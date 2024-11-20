using Helper.ChatGPT.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.ChatGPT
{
    public interface IChatGPTApi
    {
        Task<AskChatGPTResponse?> AskChatGPT(string message);
    }
}
