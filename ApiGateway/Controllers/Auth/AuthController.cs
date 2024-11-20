using ApiGateway.Middleware;
using BusinessLogic;
using DTO.Params.SecurityParam;
using DTO.Results.SecurityResult;
using Helper.AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ApiGateway.Controllers.Auth
{
    [ApiController]
    [ControllerName("Authenticate")]
    [Route("api/authenticate")]
    public class AuthController : Controller
    {
        private readonly ILogic<LoginParam, LoginResult> _loginLogic;
        private readonly ILogic<RegisterParam, RegisterResult> _registerLogic;
        private readonly ILogic<RegisterOTPParam, RegisterOTPResult> _registerOTPLogic;
        private readonly ILogic<ActiveAccountOTPParam, ActiveAccountOTPResult> _activeAccountOTPLogic;
        private readonly ILogic<RefreshTokenParam, RefreshTokenResult> _refreshTokenLogic;
        private readonly ILogic<ChangePasswordParam, ChangePasswordResult> _changePasswordLogic;
        private readonly ILogic<ForgotPasswordParam, ForgotPasswordResult> _sendEmailForgotPasswordLogic;
        private readonly ILogic<ForgotPasswordOTPParam, ForgotPasswordOTPResult> _sendEmailForgotPasswordOTPLogic;
        private readonly ILogic<ResetPasswordParam, ResetPasswordResult> _resetPasswordLogic;
        private readonly ILogic<ResetPasswordOTPParam, ResetPasswordOTPResult> _resetPasswordOTPLogic;
        private readonly IAutoMap _autoMap;
        public AuthController(ILogic<LoginParam, LoginResult> loginLogic, ILogic<RegisterParam,
            RegisterResult> registerLogic, ILogic<RefreshTokenParam, RefreshTokenResult> refreshTokenLogic,
            IAutoMap autoMap, ILogic<ChangePasswordParam, ChangePasswordResult> changePasswordLogic,
            ILogic<ForgotPasswordParam, ForgotPasswordResult> sendEmailForgotPasswordLogic,
            ILogic<ResetPasswordParam, ResetPasswordResult> resetPasswordLogic,
            ILogic<ForgotPasswordOTPParam, ForgotPasswordOTPResult> sendEmailForgotPasswordOTPLogic,
            ILogic<ResetPasswordOTPParam, ResetPasswordOTPResult> resetPasswordOTPLogic,
            ILogic<RegisterOTPParam, RegisterOTPResult> registerOTPLogic,
            ILogic<ActiveAccountOTPParam, ActiveAccountOTPResult> activeAccountOTPLogic)
        {
            _loginLogic = loginLogic;
            _registerLogic = registerLogic;
            _refreshTokenLogic = refreshTokenLogic;
            _autoMap = autoMap;
            _changePasswordLogic = changePasswordLogic;
            _sendEmailForgotPasswordLogic = sendEmailForgotPasswordLogic;
            _resetPasswordLogic = resetPasswordLogic;
            _sendEmailForgotPasswordOTPLogic = sendEmailForgotPasswordOTPLogic;
            _resetPasswordOTPLogic = resetPasswordOTPLogic;
            _registerOTPLogic = registerOTPLogic;
            _activeAccountOTPLogic = activeAccountOTPLogic;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<LoginResult?> Login(LoginParam param)
        {
            Log.Information($"loginTenant {param.Email}");

            var returnData = _loginLogic.Execute(param)?.Result;

            if (returnData == null)
                return new LoginResult
                {
                    Result = false,
                    MsgCode = "LOGIN_FAILED"
                };

            return returnData;
        }

        [HttpPost]
        [Route("register")]
        public ActionResult<RegisterResult?> Register(RegisterParam param)
        {
            Log.Information($"loginTenant {param}");

            var returnData = _registerLogic.Execute(param)?.Result;

            if (returnData == null)
                return new RegisterResult
                {
                    Result = false,
                    MsgCode = "LOGIN_FAILED"
                };

            return returnData;

        }

        [HttpPost]
        [Route("register-otp")]
        public ActionResult<RegisterOTPResult?> RegisterOTP(RegisterOTPParam param)
        {
            Log.Information($"loginTenant {param}");

            var returnData = _registerOTPLogic.Execute(param)?.Result;

            if (returnData == null)
                return new RegisterOTPResult
                {
                    Result = false,
                    MsgCode = "LOGIN_FAILED"
                };

            return returnData;

        }

        [HttpPost]
        [Route("active-account-otp")]
        public ActionResult<ActiveAccountOTPResult?> ActiveAccountOTP(ActiveAccountOTPParam param)
        {
            Log.Information($"ActiveAccountOTP {param}");

            var returnData = _activeAccountOTPLogic.Execute(param)?.Result;

            if (returnData == null)
                return new ActiveAccountOTPResult
                {
                    Result = false,
                    MsgCode = "LOGIN_FAILED"
                };

            return returnData;

        }

        [HttpGet]
        [Route("refresh-token")]
        public ActionResult<RefreshTokenResult?> RefreshToken(string refreshToken)
        {
            Log.Information($"refreshToken {refreshToken}");

            var returnData = _refreshTokenLogic.Execute(new RefreshTokenParam
            {
                RefreshToken = refreshToken
            })?.Result;

            if (returnData == null)
                return new RefreshTokenResult
                {
                    Result = false,
                    MsgCode = "REFRESH_TOKEN_FAILED"
                };

            return returnData;
        }

        [HttpPost]
        [Route("change-password/{id}")]
        [TypeFilter(typeof(AuthorizeUserAttribute))]
        public ActionResult<ChangePasswordResult?> ChangePassword([FromRoute] string id, ChangePasswordRequest request)
        {
            Log.Information($"changePassword {request}");

            var param = _autoMap.Map<ChangePasswordRequest, ChangePasswordParam>(request);
            param.Id = id;

            var returnData = _changePasswordLogic.Execute(param)?.Result;

            if (returnData == null)
                return new ChangePasswordResult
                {
                    Result = false,
                    MsgCode = "CHANGE_PASSWORD_FAILED"
                };

            return returnData;
        }

        [HttpPost]
        [Route("forgot-password")]
        public ActionResult<ForgotPasswordResult?> SendMailForgotPassword(ForgotPasswordParam param)
        {
            Log.Information($"SendMailForgotPassword {param}");

            var returnData = _sendEmailForgotPasswordLogic.Execute(param)?.Result;

            if (returnData == null)
                return new ForgotPasswordResult
                {
                    Result = false,
                    MsgCode = "RESET_PASSWORD_FAILED",
                };

            return returnData;
        }

        [HttpPost]
        [Route("forgot-password-otp")]
        public ActionResult<ForgotPasswordOTPResult?> SendMailForgotPasswordOTP(ForgotPasswordOTPParam param)
        {
            Log.Information($"SendMailForgotPassword {param}");

            var returnData = _sendEmailForgotPasswordOTPLogic.Execute(param)?.Result;

            if (returnData == null)
                return new ForgotPasswordOTPResult
                {
                    Result = false,
                    MsgCode = "RESET_PASSWORD_FAILED",
                };

            return returnData;
        }

        [HttpPost]
        [Route("reset-password")]
        public ActionResult<ResetPasswordResult?> ResetPassword(ResetPasswordParam param)
        {
            Log.Information($"ResetPassword {param}");

            var returnData = _resetPasswordLogic.Execute(param)?.Result;

            if (returnData == null)
                return new ResetPasswordResult
                {
                    Result = false,
                    MsgCode = "RESET_PASSWORD_FAILED",
                };

            return returnData;
        }

        [HttpPost]
        [Route("reset-password-otp")]
        public ActionResult<ResetPasswordOTPResult?> ResetPasswordOTP(ResetPasswordOTPParam param)
        {
            Log.Information($"ResetPasswordOTP {param}");

            var returnData = _resetPasswordOTPLogic.Execute(param)?.Result;

            if (returnData == null)
                return new ResetPasswordOTPResult
                {
                    Result = false,
                    MsgCode = "RESET_PASSWORD_FAILED",
                };

            return returnData;
        }
    }
}