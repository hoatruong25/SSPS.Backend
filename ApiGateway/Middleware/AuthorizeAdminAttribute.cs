using BusinessLogic;
using DTO.Params.UserParam;
using DTO.Results.SecurityResult;
using DTO.Results.UserResult;
using Helper.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiGateway.Middleware
{
    public class AuthorizeAdminAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IJwtToken _jwtToken;
        private readonly ILogic<GetUserParam, GetUserResult> _getUserLogic;
        public AuthorizeAdminAttribute(IJwtToken jwtToken, ILogic<GetUserParam, GetUserResult> getUserLogic)
        {
            _jwtToken = jwtToken;
            _getUserLogic = getUserLogic;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new JsonResult(new { message = "UnAuthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            var user = _jwtToken.VerifyToken(token);

            // Authorize
            if (string.IsNullOrEmpty(user?.Id) || user?.Role?.ToUpper() != "ADMIN")
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            var userDetail = _getUserLogic.Execute(new GetUserParam { Id = user?.Id != null ? user.Id : "" })?.Result;

            if (userDetail?.Result == false)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            context.HttpContext.Items["User"] = new UserContextResult
            {
                Result = true,
                Id = userDetail.Data.Id,
                Code = userDetail.Data.Code,
            };
        }
    }
}
