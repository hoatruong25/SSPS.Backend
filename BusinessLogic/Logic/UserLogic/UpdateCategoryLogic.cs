using DTO.Params.UserParam;
using DTO.Results.UserResult;
using Helper.AutoMapper;
using Infrastructure.PgModels;
using Microsoft.IdentityModel.Tokens;
using Repository.PgReposiotries.PgCategoryRepo;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.UserLogic
{
    public class UpdateCategoryLogic : ILogic<UpdateCategoryParam, UpdateCategoryResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IPgCategoryRepository _categoryRepository;
        private readonly IAutoMap _autoMap;
        public UpdateCategoryLogic(IPgUserRepository userRepository, IPgCategoryRepository categoryRepository, IAutoMap autoMap)
        {
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _autoMap = autoMap;
        }
        public async Task<UpdateCategoryResult>? Execute(UpdateCategoryParam param)
        {
            Log.Information($"UpdateCategoryLogic Param: {param}");

            var returnData = new UpdateCategoryResult
            {
                Result = false,
                MsgCode = "UPDATE_CATEGORY_FAILED",
            };

            try
            {
                var user = await _userRepository.GetUser(param.UserId);

                if (user == null)
                {
                    returnData.MsgCode = "USER_NOT_FOUND";
                    return returnData;
                }

                var categories = new List<PgCategoryUsageMoney>();

                foreach (var category in param.Categories)
                {
                    categories.Add(new PgCategoryUsageMoney
                    {
                        Id = category.Id.IsNullOrEmpty() ? Guid.NewGuid() : Guid.Parse(category.Id),
                        UserId = user.Id,
                        Name = category.Name,
                        IsDefault = category.IsDefault,
                    });
                }

                await _categoryRepository.DeleteCategoryByUserId(param.UserId);
                await _categoryRepository.CreateListCategory(categories);

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new UpdateCategoryDataResult();

                return returnData;

            }
            catch (Exception ex)
            {
                Log.Information($"UpdateCategoryLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}