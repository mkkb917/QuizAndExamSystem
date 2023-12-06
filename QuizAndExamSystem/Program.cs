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
using Serilog;
using Microsoft.AspNetCore.Hosting;
using ExamSystem;
using System.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using ExamSystem.Data.Static;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;
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
builder.Services.AddScoped<IPaperService, PaperService>();
builder.Services.AddScoped<IQuizService, QuizService>();

//email sending service
builder.Services.AddTransient<IEmailService>(s=> new EmailService("smtp.ethereal.email", 587, "no-reply@opegs.com"));

//DinktoPDF services
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddTransient<IRazorPartialToStringRenderer, RazorPartialToStringRenderer>();

// Google ReCaptcha 
builder.Services.AddReCaptcha(builder.Configuration.GetSection("GoogleReCaptcha"));


//Authentication and Authorization
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

// forget password token generation service configuration
// token time validity for forget password service  (Timeset = 2 hours)
builder.Services.Configure<DataProtectionTokenProviderOptions>(option => option.TokenLifespan = TimeSpan.FromHours(2));
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
    .AddCookie();   //o=> o.LoginPath="/Auth/Login");   for custom login path, default is account/login
    
//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

//external logins setting using Google
builder.Services.AddAuthentication().AddGoogle(option =>
{
    option.ClientId = "1011957018314-9vcp85dcqlk3rrqhbai3989k319d1su0.apps.googleusercontent.com";
    option.ClientSecret = "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";
    option.SignInScheme = IdentityConstants.ExternalScheme;
});

//external login setting using Facebook
//builder.Services.AddAuthentication().AddFacebook(option =>
//{
//    option.AppId = Configuration["Facebook:AppId"];     // stored in Secrets.json file 
//    option.AppSecret = Configuration["Facebook:AppSecret"];
//});

//limitize the pasword complexity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedEmail = true;
});



//DBContext configuration with sql server connection string
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
// BreadCrumbs for site

//log setting
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.File(Path.Combine(environment.WebRootPath+WC.logfilePath, "log-.txt"), rollingInterval: RollingInterval.Month)
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog for logging


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

