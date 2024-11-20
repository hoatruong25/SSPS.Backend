using ApiGateway.Middleware;
using BusinessLogic;
using BusinessLogic.Logic.MoneyPlanLogic;
using DTO.Params.ExternalParam;
using DTO.Params.MoneyPlanParam;
using DTO.Params.SecurityParam;
using DTO.Results.ExternalResult;
using DTO.Results.MoneyPlanResult;
using DTO.Results.SecurityResult;
using Helper.AutoMapper;
using Helper.FirebaseNoti;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ApiGateway.Controllers.External
{
    [ApiController]
    [ControllerName("External")]
    [Route("api/external")]
    public class TestController : Controller
    {
        private readonly INotificationService _notificationService;
        public TestController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("send-noti")]
        public ActionResult<string> AskChatGPT([FromQuery] string deviceToken)
        {
            _notificationService.SendNotification("Test notification", "Test notification", deviceToken, new Dictionary<string, string>());

            return "Oke";
        }

    }
}