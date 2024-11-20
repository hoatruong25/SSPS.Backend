using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO.Params.UserParam;
using DTO.Results.UserResult;
using Infrastructure.Models;

namespace Repository.Repositories.ForgotPasswordRepo
{
    public interface IForgotPasswordRepository
    {
        Task<ForgotPassword> CreateForgotPasswordAsync(ForgotPassword forgotPassword);
        Task<ForgotPassword?> GetForgotPasswordByTokenAsync(string token);
        Task<bool> UpdateForgotPassword(ForgotPassword forgotPassword);
    }
}