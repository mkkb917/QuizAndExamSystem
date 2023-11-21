using AspNetCore.ReCaptcha;
using DinkToPdf;
using DinkToPdf.Contracts;
using ExamSystem.Data;
using ExamSystem.Data.Interface;
using ExamSystem.Data.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using ExamSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container 

// Add filters to controllers and methods such as [Authorize]
builder.Services.AddControllersWithViews(/*q=>q.Filters.Add(new AuthorizeFilter())*/)
        .AddNewtonsoftJson(options =>
        {                               //newtonsoft injection for enums text display
            options.SerializerSettings.Converters.Add(new StringEnumConverter()); 
        }); 
builder.Services.AddMvc().AddControllersAsServices();
//Add services 
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ISEDService, SEDService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUploadsService, UploadsService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IQuizService, QuizService>();

//DinktoPDF services
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddTransient<IRazorPartialToStringRenderer, RazorPartialToStringRenderer>();

// Google ReCaptcha 
builder.Services.AddReCaptcha(builder.Configuration.GetSection("GoogleReCaptcha"));


//Authentication and Authorization
//builder.Services.AddAccount<ApplicationUser, AccountRole>().AddEntityFrameworkStores<AppDbContext>();
// forget password token generation service configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
// token time validity for forget password service  (Timeset = 10 hours)
builder.Services.Configure<DataProtectionTokenProviderOptions>(option => option.TokenLifespan = TimeSpan.FromHours(10));
builder.Services.AddMemoryCache();

///// CREATES LOGIN USER ISSUES 
//builder.Services.AddSession(option =>
//{
//    option.Cookie.IsEssential = true;
//    option.Cookie.SameSite = SameSiteMode.Strict;
//    option.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
//    option.IdleTimeout = TimeSpan.FromMinutes(30);
//});
// use cookie authentication   
//builder.Services.AddCookiePolicy(options =>
//{
//    options.MinimumSameSitePolicy = SameSiteMode.None;
//    options.Secure = CookieSecurePolicy.SameAsRequest;
//    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;    
//});
//builder.Services.AddAuthentication(option =>
//{
//    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;   
//});
builder.Services.AddAuthentication();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();   //q=> q.LoginPath="/Auth/Login");   for custom login path, default is account/login

//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
// external logins setting using Google
//builder.Services.AddAuthentication().AddGoogle(option =>
//{
//    option.ClientId = "1011957018314-9vcp85dcqlk3rrqhbai3989k319d1su0.apps.googleusercontent.com";
//    option.ClientSecret = "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";
//    option.SignInScheme = AccountConstants.ExternalScheme;
//});

//external login setting using Facebook
//builder.Services.AddAuthentication().AddFacebook(option =>
//{
//    option.AppId = "1275553409642050";
//    option.AppSecret = "8e9dc647c729adab898188f83c5104cb";
//});

//limitize the pasword complexity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

//DBContext configuration with sql server connection string
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
// BreadCrumbs for site



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseCookiePolicy();
app.UseStaticFiles();

app.UseRouting();
//app.UseSession();

app.UseCors("corsapp");
//Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// seed database with default userdata
await AppDbInitializer.SeedUsersAndRollAsync(app);

app.Run();

