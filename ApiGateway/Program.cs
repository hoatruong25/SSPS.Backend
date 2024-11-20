using ApiGateway.Middleware;
using AutoMapper;
using BusinessLogic;
using BusinessLogic.Logic.AuthenticationLogic;
using BusinessLogic.Logic.UserLogic;
using DTO.Params.SecurityParam;
using DTO.Params.UserParam;
using DTO.Results.SecurityResult;
using DTO.Results.UserResult;
using Helper.AutoMapper;
using Helper.Security;
using Microsoft.OpenApi.Models;
using Infrastructure.Models;
using Repository.Repositories.UserRepo;
using Repository.Repositories.ForgotPasswordRepo;
using DTO.Params.MoneyPlanParam;
using DTO.Results.MoneyPlanResult;
using BusinessLogic.Logic.MoneyPlanLogic;
using Repository.Repositories.MoneyPlanRepo;
using DTO.Params.NoteParam;
using DTO.Results.NoteResult;
using BusinessLogic.Logic.NoteLogic;
using Repository.Repositories.NoteRepo;
using Repository.Repositories.ToDoNoteRepo;
using DTO.Results.ToDoNoteResult;
using DTO.Params.ToDoNoteParam;
using BusinessLogic.Logic.ToDoNoteLogic;
using DTO.Results.AdminResult;
using BusinessLogic.Logic.Caching;
using Helper.Paypal;
using Helper.ChatGPT;
using DTO.Params.ExternalParam;
using DTO.Results.ExternalResult;
using BusinessLogic.Logic.ExternalLogic;
using Infrastructure.PgModels;
using Microsoft.EntityFrameworkCore;
using Repository.PgReposiotries.PgForgotPasswordRepo;
using Repository.PgReposiotries.PgUserRepo;
using Repository.PgReposiotries.PgMoneyPlanRepo;
using Repository.PgReposiotries.PgNoteRepo;
using Repository.PgReposiotries.PgToDoNoteRepo;
using Repository.PgReposiotries.PgUsageRepo;
using Repository.PgReposiotries.PgToDoCardRepo;
using Repository.PgReposiotries.PgCategoryRepo;
using Helper.FirebaseNoti;
using Hangfire;
using Hangfire.PostgreSql;
using Helper.SendEmail;
using Repository.PgReposiotries.PgOTPRepo;
using Repository.PgReposiotries.PgChatBoxDataRepo;

var builder = WebApplication.CreateBuilder(args);

// Fix CORS issue
builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                });
            });

// Add services to the container.
void RegisterRepository()
{
    builder.Services.Configure<SSPSDataSettings>(builder.Configuration.GetSection("SSPSDatabase"));
    builder.Services.AddTransient<IUserRepository, UserRepository>();
    builder.Services.AddTransient<IForgotPasswordRepository, ForgotPasswordRepository>();
    builder.Services.AddTransient<IMoneyPlanRepository, MoneyPlanRepository>();
    builder.Services.AddTransient<INoteRepository, NoteRepository>();
    builder.Services.AddTransient<IToDoNoteRepository, ToDoNoteRepository>();
}

void RegisterPgRepository()
{
    builder.Services.AddTransient<IPgUserRepository, PgUserRepository>();
    builder.Services.AddTransient<IPgForgotPasswordRepository, PgForgotPasswordRepository>();
    builder.Services.AddTransient<IPgMoneyPlanRepository, PgMoneyPlanRepository>();
    builder.Services.AddTransient<IPgNoteRepository, PgNoteRepository>();
    builder.Services.AddTransient<IPgToDoNoteRepository, PgToDoNoteRepository>();
    builder.Services.AddTransient<IPgToDoCardRepository, PgToDoCardRepository>();
    builder.Services.AddTransient<IPgUsageRepository, PgUsageRepository>();
    builder.Services.AddTransient<IPgCategoryRepository, PgCategoryRepository>();
    builder.Services.AddTransient<IPgOTPRepository, PgOTPRepository>();
    builder.Services.AddTransient<IPgChatBoxDataRepository, PgChatBoxDataRepository>();
}

void RegisterLogic()
{
    builder.Services.AddScoped<ILogic<LoginParam, LoginResult>, LoginLogic>();
    builder.Services.AddScoped<ILogic<RegisterParam, RegisterResult>, RegisterLogic>();
    builder.Services.AddScoped<ILogic<RegisterOTPParam, RegisterOTPResult>, RegisterOTPLogic>();
    builder.Services.AddScoped<ILogic<ActiveAccountOTPParam, ActiveAccountOTPResult>, ActiveAccountOTPLogic>();
    builder.Services.AddScoped<ILogic<RefreshTokenParam, RefreshTokenResult>, RefreshTokenLogic>();
    builder.Services.AddScoped<ILogic<ChangePasswordParam, ChangePasswordResult>, ChangePasswordLogic>();
    builder.Services.AddScoped<ILogic<ForgotPasswordParam, ForgotPasswordResult>, SendEmailForgotPasswordLogic>();
    builder.Services.AddScoped<ILogic<ForgotPasswordOTPParam, ForgotPasswordOTPResult>, SendEmailForgotPasswordOTPLogic>();
    builder.Services.AddScoped<ILogic<ResetPasswordParam, ResetPasswordResult>, ResetPasswordLogic>();
    builder.Services.AddScoped<ILogic<ResetPasswordOTPParam, ResetPasswordOTPResult>, ResetPasswordOTPLogic>();
    builder.Services.AddScoped<ILogic<GetDashboardParam, GetDashboardResult>, GetDashboardLogic>();

    // UserLogic
    builder.Services.AddScoped<ILogic<GetUserParam, GetUserResult>, GetUserLogic>();
    builder.Services.AddScoped<ILogic<GetListUserParam, GetListUserResult>, GetListUserLogic>();
    builder.Services.AddScoped<ILogic<GetUserDetailParam, GetUserDetailResult>, GetUserDetailLogic>();
    builder.Services.AddScoped<ILogic<UpdateUserParam, UpdateUserResult>, UpdateUserLogic>();
    builder.Services.AddScoped<ILogic<UpdateCategoryParam, UpdateCategoryResult>, UpdateCategoryLogic>();
    builder.Services.AddScoped<ILogic<DeleteCategoryParam, DeleteCategoryResult>, DeleteCategoryLogic>();

    // MoneyPlan Logic
    builder.Services.AddScoped<ILogic<CreateMoneyPlanParam, CreateMoneyPlanResult>, CreateMoneyPlanLogic>();
    builder.Services.AddScoped<ILogic<CreateListMoneyPlanParam, CreateListMoneyPlanResult>, CreateListMoneyPlanLogic>();
    builder.Services.AddScoped<ILogic<GetMoneyPlanParam, GetMoneyPlanResult>, GetMoneyPlanLogic>();
    builder.Services.AddScoped<ILogic<GetListMoneyPlanParam, GetListMoneyPlanResult>, GetListMoneyPlanLogic>();
    builder.Services.AddScoped<ILogic<UpdateUsageMoneyParam, UpdateUsageMoneyResult>, UpdateUsageMoneyLogic>();
    builder.Services.AddScoped<ILogic<UpdateMoneyPlanParam, UpdateMoneyPlanResult>, UpdateMoneyPlanLogic>();
    builder.Services.AddScoped<ILogic<GetListCategoryParam, GetListCategoryResult>, GetListCategoryLogic>();
    builder.Services.AddScoped<ILogic<DeleteMoneyPlanParam, DeleteMoneyPlanResult>, DeleteMoneyPlanLogic>();

    // Note Logic
    builder.Services.AddScoped<ILogic<CreateNoteParam, CreateNoteResult>, CreateNoteLogic>();
    builder.Services.AddScoped<ILogic<UpdateNoteParam, UpdateNoteResult>, UpdateNoteLogic>();
    builder.Services.AddScoped<ILogic<DeleteNoteParam, DeleteNoteResult>, DeleteNoteLogic>();
    builder.Services.AddScoped<ILogic<GetListNoteInRangeParam, GetListNoteInRangeResult>, GetListNoteInRangeLogic>();

    // ToDoNote Logic
    builder.Services.AddScoped<ILogic<CreateToDoNoteParam, CreateToDoNoteResult>, CreateToDoNoteLogic>();
    builder.Services.AddScoped<ILogic<UpdateToDoNoteParam, UpdateToDoNoteResult>, UpdateToDoNoteLogic>();
    builder.Services.AddScoped<ILogic<DeleteToDoNoteParam, DeleteToDoNoteResult>, DeleteToDoNoteLogic>();
    builder.Services.AddScoped<ILogic<GetAllToDoNoteParam, GetAllToDoNoteResult>, GetAllToDoNoteLogic>();
    builder.Services.AddScoped<ILogic<GetToDoNoteParam, GetToDoNoteResult>, GetToDoNoteLogic>();

    // ToDoCard Logic
    builder.Services.AddScoped<ILogic<CreateToDoCardParam, CreateToDoCardResult>, CreateToDoCardLogic>();
    builder.Services.AddScoped<ILogic<UpdateToDoCardParam, UpdateToDoCardResult>, UpdateToDoCardLogic>();
    builder.Services.AddScoped<ILogic<DeleteToDoCardParam, DeleteToDoCardResult>, DeleteToDoCardLogic>();
    builder.Services.AddScoped<ILogic<SwapToDoCardParam, SwapToDoCardResult>, SwapToDoCardLogic>();

    // Dashboard
    builder.Services.AddScoped<ILogic<DashboardUserParam, DashboardUserResult>, GetDashboardUserLogic>();

    // External
    builder.Services.AddScoped<ILogic<AskChatGptParam, AskChatGptResult>, AskChatGptLogic>();

}

void RegisterLibraries()
{
    // Auto Mapper
    builder.Services.AddScoped<IAutoMap, AutoMap>();
    builder.Services.AddScoped<IMapperConfig, MapperConfig>();
    var mapperConfig = new MapperConfig();
    builder.Services.AddSingleton<Mapper>(mapperConfig.InitializeAutomapper());

    // Paypal
    builder.Services.AddScoped<IPayPalApi, PayPalApi>();

    // ChatGPT
    builder.Services.AddScoped<IChatGPTApi, ChatGPTApi>();
    builder.Services.AddScoped<INotificationService, NotificationService>();

    // Send Email
    builder.Services.AddScoped<ISendEmail, SendEmail>();

    builder.Services.AddScoped<IJwtToken, JwtToken>();
    builder.Services.AddTransient<AuthorizeUserAttribute>();
    builder.Services.AddTransient<AuthorizeAdminAttribute>();
    builder.Services.AddScoped<HttpClient, HttpClient>();
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "SFA API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Config Redis Server
builder.Services.AddStackExchangeRedisCache(redisOption =>
{
    var connection = builder.Configuration.GetSection("SSPSDatabase")["Redis"];
    redisOption.Configuration = connection;
});

// Config Hangfire
builder.Services.AddHangfire((sp, config) =>
{
    var pgConnectionString = builder.Configuration.GetSection("SSPSDatabase")["PgConnectionString"];
    config.UsePostgreSqlStorage(pgConnectionString);
});

builder.Services.AddHangfireServer();

// Config PostgreSQL

builder.Services.AddDbContext<PgDbContext>(o => o.UseNpgsql(builder.Configuration.GetSection("SSPSDatabase")["PgConnectionString"]));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<ICacheService, CacheService>();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

RegisterRepository();

RegisterPgRepository();

RegisterLogic();

RegisterLibraries();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// UI Hangfire
app.UseHangfireDashboard();

app.Run();
