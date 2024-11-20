using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Results.ExternalResult
{
    public class AskChatGptResult : IResult
    {
        public AskChatGptDataResult? Data { get; set; }
    }
    public class AskChatGptDataResult
    {
        public string Answer { get; set; } = null!;
    }
}
