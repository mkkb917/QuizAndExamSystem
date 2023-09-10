using ExamSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        //default constructor
        public AppDbContext()
        {

        }

        //constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }



        //override method
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuizAttempt>(eb =>
            {
                eb.HasNoKey();
                eb.ToView(null);
            });

            modelBuilder.Entity<QuizReport>(eb =>
            {
                eb.HasNoKey();
                eb.ToView(null);
            });

            // set the id as PK in UserInfo
            modelBuilder.Entity<SchoolInfo>()
                        .HasKey(u => u.Id);
            //create one to one relation between ApplicationUser and UserInfo
            modelBuilder.Entity<SchoolInfo>()
                        .HasOne(a => a.AppUser)
                        .WithOne(u => u.SchoolInfo)
            //            //.HasPrincipalKey(au => au.UserName)
                        .HasForeignKey<SchoolInfo>(up => up.Id);


            base.OnModelCreating(modelBuilder);

        }
        
        

        //defining table names for each model
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<QuestionMeta> QuestionMetas { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<SchoolInfo> SchoolInfos { get; set; }
        public DbSet<Marks> Marks { get; set; }
        public DbSet<PaperSetting> PaperSettings { get; set; }
        public DbSet<GeneratedPaper> GeneratedPapers { get; set; }
        public DbSet<Notification> Notification { get; set; } // table need to be droped 

        
        public DbSet<SED> SED { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Uploads> Uploads { get; set; }
        public DbSet<Quiz> Quizes { get; set; }

        


    }

    
}
