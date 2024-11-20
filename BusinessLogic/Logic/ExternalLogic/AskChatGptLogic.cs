using DTO.Params.ExternalParam;
using DTO.Params.ToDoNoteParam;
using DTO.Results.ExternalResult;
using DTO.Results.ToDoNoteResult;
using Helper.AutoMapper;
using Helper.ChatGPT;
using Infrastructure.Models;
using MongoDB.Bson;
using Repository.Repositories.ToDoNoteRepo;
using Serilog;

namespace BusinessLogic.Logic.ExternalLogic
{
    public class AskChatGptLogic : ILogic<AskChatGptParam, AskChatGptResult>
    {
        private readonly IChatGPTApi _chatGPTApi;
        public AskChatGptLogic(IChatGPTApi chatGPTApi)
        {
            _chatGPTApi = chatGPTApi;
        }
        public async Task<AskChatGptResult>? Execute(AskChatGptParam param)
        {
            var returnData = new AskChatGptResult
            {
                Result = false,
            };

            try
            {
                var resultData = await _chatGPTApi.AskChatGPT(param.Message);

                if (resultData == null)
                    return returnData;

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new AskChatGptDataResult
                {
                    Answer = resultData.text,
                };

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"Call Chat GPT fail: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}