using DTO.Params.UserParam;
using DTO.Results.UserResult;
using Helper.AutoMapper;
using Infrastructure.PgModels;
using Repository.PgReposiotries.PgCategoryRepo;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.UserLogic
{
    public class DeleteCategoryLogic : ILogic<DeleteCategoryParam, DeleteCategoryResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IPgCategoryRepository _categoryRepository;
        private readonly IAutoMap _autoMap;
        public DeleteCategoryLogic(IPgUserRepository userRepository, IPgCategoryRepository categoryRepository, IAutoMap autoMap)
        {
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _autoMap = autoMap;
        }
        public async Task<DeleteCategoryResult>? Execute(DeleteCategoryParam param)
        {
            Log.Information($"DeleteCategoryLogic Param: {param}");

            var returnData = new DeleteCategoryResult
            {
                Result = false,
                MsgCode = "DELETE_CATEGORY_FAILED",
            };

            try
            {
                await _categoryRepository.DeleteCategoryById(param.Id);

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new DeleteCategoryDataResult();

                return returnData;

            }
            catch (Exception ex)
            {
                Log.Information($"DeleteCategoryLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}