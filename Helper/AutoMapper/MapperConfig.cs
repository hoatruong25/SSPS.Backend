using AutoMapper;
using DTO.Params.MoneyPlanParam;
using DTO.Params.NoteParam;
using DTO.Params.SecurityParam;
using DTO.Params.ToDoNoteParam;
using DTO.Params.UserParam;
using DTO.Results.MoneyPlanResult;
using DTO.Results.NoteResult;
using DTO.Results.ToDoNoteResult;
using DTO.Results.UserResult;
using Infrastructure.Models;
using Infrastructure.PgModels;
using MongoDB.Bson;

namespace Helper.AutoMapper
{
    public class MapperConfig : IMapperConfig
    {
        public Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // PG DATABASE CONFIG
                // Start
                // Start User
                cfg.CreateMap<RegisterParam, PgUser>();
                cfg.CreateMap<RegisterOTPParam, PgUser>();
                cfg.CreateMap<ChangePasswordRequest, ChangePasswordParam>();

                cfg.CreateMap<PgUser, GetUserDetailDataResult>();
                cfg.CreateMap<UpdateUserRequest, UpdateUserParam>();

                cfg.CreateMap<UpdateCategoryRequest, UpdateCategoryParam>();
                cfg.CreateMap<DashboardUserRequest, DashboardUserParam>();
                cfg.CreateMap<DeleteCategoryRequest, DeleteCategoryParam>();
                cfg.CreateMap<UpdateCategoryDataRequest, PgCategoryUsageMoney>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                // End User

                // Start Money Plan
                cfg.CreateMap<CreateMoneyPlanRequest, CreateMoneyPlanParam>();
                cfg.CreateMap<CreateListMoneyPlanRequest, CreateListMoneyPlanParam>();
                cfg.CreateMap<GetListMoneyPlanRequest, GetListMoneyPlanParam>();
                cfg.CreateMap<PgUsageMoney, GetMoneyPlanDataUsageMoneyResult>();

                cfg.CreateMap<PgUsageMoney, GetMoneyPlanDataUsageMoneyResult>().ReverseMap();
                cfg.CreateMap<PgMoneyPlan, GetMoneyPlanDataResult>();

                cfg.CreateMap<PgUsageMoney, GetListMoneyPlanDataUsageMoneyResult>().ReverseMap();
                cfg.CreateMap<PgMoneyPlan, GetListMoneyPlanDataResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id));

                cfg.CreateMap<UpdateUsageMoneyDataParam, PgUsageMoney>();

                cfg.CreateMap<UpdateUsageMoneyRequest, UpdateUsageMoneyRequest>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(s => s.Data));

                cfg.CreateMap<UpdateMoneyPlanRequest, UpdateMoneyPlanParam>();
                // End Money Plan

                // Start Note
                cfg.CreateMap<CreateNoteRequest, CreateNoteParam>();
                cfg.CreateMap<UpdateNoteRequest, UpdateNoteParam>();
                cfg.CreateMap<DeleteNoteRequest, DeleteNoteParam>();
                cfg.CreateMap<GetListNoteInRangeRequest, GetListNoteInRangeParam>();
                cfg.CreateMap<PgNote, GetListNoteInRangeDataResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id));
                // End Note

                // Start ToDoNote
                cfg.CreateMap<CreateToDoNoteRequest, CreateToDoNoteParam>();
                cfg.CreateMap<UpdateToDoNoteRequest, UpdateToDoNoteParam>();
                cfg.CreateMap<DeleteToDoNoteRequest, DeleteToDoNoteParam>();
                cfg.CreateMap<CreateToDoNoteCardParam, PgToDoCard>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ObjectId.GenerateNewId()));
                cfg.CreateMap<UpdateToDoNoteCardParam, PgToDoCard>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ObjectId.GenerateNewId()));
                cfg.CreateMap<PgToDoCard, GetAllToDoNoteCardDataResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id));
                cfg.CreateMap<PgToDoNote, GetAllToDoNoteDataResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id));
                cfg.CreateMap<PgToDoCard, GetToDoNoteCardDataResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id));
                cfg.CreateMap<PgToDoNote, GetToDoNoteDataResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id));
                // End ToDoNote

                // Start ToDoCard
                cfg.CreateMap<CreateToDoCardRequest, CreateToDoCardParam>();
                cfg.CreateMap<UpdateToDoCardRequest, UpdateToDoCardParam>();
                cfg.CreateMap<DeleteToDoCardRequest, DeleteToDoCardParam>();
                cfg.CreateMap<PgToDoCard, GetToDoNoteCardDataResult>();
                cfg.CreateMap<PgToDoCard, GetAllToDoNoteCardDataResult>();
                // End ToDoCard
                // End

                // MONGODB CONFIG
                // Start User
                cfg.CreateMap<RegisterParam, PgUser>();
                cfg.CreateMap<ChangePasswordRequest, ChangePasswordParam>();

                cfg.CreateMap<PgUser, GetUserDetailDataResult>();
                cfg.CreateMap<UpdateUserRequest, UpdateUserParam>();

                cfg.CreateMap<UpdateCategoryRequest, UpdateCategoryParam>();
                cfg.CreateMap<UpdateCategoryDataRequest, CategoryUsageMoney>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => ObjectId.GenerateNewId()))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                // End User

                // Start Money Plan
                cfg.CreateMap<CreateMoneyPlanRequest, CreateMoneyPlanParam>();
                cfg.CreateMap<GetListMoneyPlanRequest, GetListMoneyPlanParam>();

                cfg.CreateMap<UsageMoney, GetMoneyPlanDataUsageMoneyResult>().ReverseMap();
                cfg.CreateMap<MoneyPlan, GetMoneyPlanDataResult>();

                cfg.CreateMap<UsageMoney, GetListMoneyPlanDataUsageMoneyResult>().ReverseMap();
                cfg.CreateMap<MoneyPlan, GetListMoneyPlanDataResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s._id));

                cfg.CreateMap<UpdateUsageMoneyDataParam, UsageMoney>();

                cfg.CreateMap<UpdateUsageMoneyRequest, UpdateUsageMoneyRequest>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(s => s.Data));

                cfg.CreateMap<UpdateMoneyPlanRequest, UpdateMoneyPlanParam>();
                // End Money Plan

                // Start Note
                cfg.CreateMap<CreateNoteRequest, CreateNoteParam>();
                cfg.CreateMap<UpdateNoteRequest, UpdateNoteParam>();
                cfg.CreateMap<DeleteNoteRequest, DeleteNoteParam>();
                cfg.CreateMap<GetListNoteInRangeRequest, GetListNoteInRangeParam>();
                cfg.CreateMap<Note, GetListNoteInRangeDataResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s._id));
                // End Note

                // Start ToDoNote
                cfg.CreateMap<CreateToDoNoteRequest, CreateToDoNoteParam>();
                cfg.CreateMap<UpdateToDoNoteRequest, UpdateToDoNoteParam>();
                cfg.CreateMap<DeleteToDoNoteRequest, DeleteToDoNoteParam>();
                cfg.CreateMap<CreateToDoNoteCardParam, ToDoCard>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => ObjectId.GenerateNewId()));
                cfg.CreateMap<UpdateToDoNoteCardParam, ToDoCard>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => ObjectId.GenerateNewId()));
                cfg.CreateMap<ToDoCard, GetAllToDoNoteCardDataResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s._id));
                cfg.CreateMap<ToDoNote, GetAllToDoNoteDataResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s._id));
                cfg.CreateMap<ToDoCard, GetToDoNoteCardDataResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s._id));
                cfg.CreateMap<ToDoNote, GetToDoNoteDataResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s._id));
                // End ToDoNote

                // Start ToDoCard
                cfg.CreateMap<CreateToDoCardRequest, CreateToDoCardParam>();
                cfg.CreateMap<UpdateToDoCardRequest, UpdateToDoCardParam>();
                cfg.CreateMap<DeleteToDoCardRequest, DeleteToDoCardParam>();
                // End ToDoCard
            });
            // Create an Instance of mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}