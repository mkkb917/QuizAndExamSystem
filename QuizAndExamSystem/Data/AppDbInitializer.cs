using ExamSystem.Data.Static;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamSystem.Models;

namespace ExamSystem.Data
{

    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

                //Add default data to each class

                //Grade
                if (!context.Grades.Any())
                {
                    context.Grades.AddRange(new List<Grade>()
                    {
                        new Grade()
                        {
                            Code    ="G-1",
                            Status = Status.Pending,
                            GradeText = "Grade 1",
                            Description = " This is test Grade 1",
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedBy = string.Empty,
                            UpdatedOn = DateTime.Now,
                        },
                        new Grade()
                        {
                            Code    ="G-2",
                            Status = Status.Pending,
                            GradeText = "Grade 2",
                            Description = " This is test Grade 2",
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedBy = string.Empty,
                            UpdatedOn = DateTime.Now,
                        },
                        new Grade()
                        {
                            Code    ="G-3",
                            Status = Status.Pending,
                            GradeText = "Grade 3",
                            Description = " This is test Grade 3",
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedBy = string.Empty,
                            UpdatedOn = DateTime.Now,
                        }
                    }) ;
                }
                //save the changes to database
                context.SaveChanges();
                //Subject
                if (!context.Subjects.Any())
                {
                    context.Subjects.AddRange(new List<Subject>()
                    {
                    new Subject()
                        {
                            Code    ="SUB-1",
                            Status = Status.Pending,
                            SubjectText = "Subject 1",
                            Description = " This is test Subject 1",
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedBy = string.Empty,
                            UpdatedOn = DateTime.Now,
                            GradeId   = 1
                        },
                    new Subject()
                        {
                        Code    ="SUB-2",
                            Status = Status.Pending,
                            SubjectText = "Subject 2",
                            Description = " This is test Subject 2",
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedBy = string.Empty,
                            UpdatedOn = DateTime.Now,
                            GradeId   = 1
                        },
                    new Subject()
                        {
                        Code    ="SUB-3",
                            Status = Status.Pending,
                            SubjectText = "Subject 3",
                            Description = " This is test Subject 3",
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedBy = string.Empty,
                            UpdatedOn = DateTime.Now,
                            GradeId   = 1
                        }
                    });
                }
                //save the changes to database
                context.SaveChanges();
                //Topic
                if (!context.Topics.Any())
                {
                    context.Topics.AddRange(new List<Topic>()
                    {
                        new Topic()
                        {
                            TopicText = "Unit 1",
                            Code       = "Unit 1",
                            Description = " This is test Topic or Unit 1",
                            Status=Status.Pending,
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedBy = string.Empty,
                            UpdatedOn = DateTime.Now,
                            SubjectId = 1
                        },
                        new Topic()
                        {
                            TopicText = "Unit 2",
                            Code       = "Unit 2",
                            Description = " This is test Topic 2 or Unit 2",
                            Status=Status.Pending,
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedBy = string.Empty,
                            UpdatedOn = DateTime.Now,
                            SubjectId = 1
                        },
                        new Topic()
                        {
                            TopicText = "Unit 3",
                            Code       = "Unit 3",
                            Description = " This is test Topic 3 or Unit 3",
                            Status=Status.Pending,
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedBy = string.Empty,
                            UpdatedOn = DateTime.Now,
                            SubjectId = 1
                        }
                    });
                }
                //save the changes to database
                context.SaveChanges();
                //Question
                if (!context.Questions.Any())
                {
                    context.Questions.AddRange(new List<Question>()
                    {
                        new Question()
                        {
                            Code    ="Question 1",
                            Status = Status.Pending,
                            QuestionText = "What is a question?",
                            QuestionTextL = "کیا یہ ایک سوال ہے؟",
                            Description = " Question 1 of the Unit 1 of Subject 1 ",
                            QuestionType= QuestionTypes.MCQ,
                            DifficultyLevel= DifficultyLevel.Very_Easy,
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedBy = string.Empty,
                            UpdatedOn = DateTime.Now,
                            TopicId   = 1
                        },
                        new Question()
                        {
                            Code    ="Question 2",
                            Status = Status.Pending,
                            QuestionText = "What is your name?",
                            QuestionTextL = "آپ کا کیا نام ہے؟",
                            Description = " Question 2 of the Unit 1 of Subject 1 ",
                            QuestionType= QuestionTypes.SEQ,
                            DifficultyLevel= DifficultyLevel.Very_Easy,
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedBy = string.Empty,
                            UpdatedOn = DateTime.Now,
                            TopicId   = 1
                        },
                        new Question()
                        {
                            Code    ="Question 3",
                            Status = Status.Pending,
                            QuestionText = "Mouse is a(an) ________ device",
                            QuestionTextL = "ماؤس ایک ــــــ آلہ ہے۔",
                            Description = " Question 3 of the Unit 1 of Subject 1 ",
                            QuestionType= QuestionTypes.Fill_In_The_Blanks,
                            DifficultyLevel= DifficultyLevel.Very_Easy,
                            CreatedOn = DateTime.Now,
                            CreatedBy = "Admin",
                            UpdatedBy = string.Empty,
                            UpdatedOn = DateTime.Now,
                            TopicId   = 1
                        }
                    });
                }
                //save the changes to database
                context.SaveChanges();
                //Choice
                if (!context.Choices.Any())
                {
                    context.Choices.AddRange(new List<Choice>()
                    {
                        new Choice()
                        {
                            Choice1 = "Yes",
                            ChoiceL1 = "ہاں",
                            Choice2 = "No",
                            ChoiceL2 = "نہیں",
                            Choice3 = "Do not know",
                            ChoiceL3 = "نہیں معلوم",
                            Choice4 = "None of these",
                            ChoiceL4 = "کوئی نہیں",
                            Description = " 4 choices of question 1 of the Unit 1 of Subject 1 ",
                            Answer = "Yes",
                            AnswerL = "ہاں",
                            QuestionId = 1
                        },
                        new Choice()
                        {
                            Choice1 = String.Empty,
                            ChoiceL1 = String.Empty,
                            Choice2 = String.Empty,
                            ChoiceL2 =String.Empty,
                            Choice3 = String.Empty,
                            ChoiceL3 = String.Empty,
                            Choice4 = String.Empty,
                            ChoiceL4 = String.Empty,
                            Description = " only answer of short exam question 1 of the Unit 1 of Subject 1 ",
                            Answer = "My name is User",
                            AnswerL = "میرا نام یوزر ہے",
                            QuestionId = 2
                        },
                        new Choice()
                        {
                            Choice1 = String.Empty,
                            ChoiceL1 = String.Empty,
                            Choice2 = String.Empty,
                            ChoiceL2 =String.Empty,
                            Choice3 = String.Empty,
                            ChoiceL3 = String.Empty,
                            Choice4 = String.Empty,
                            ChoiceL4 = String.Empty,
                            Description = " only answer of fill in the blank exam question 1 of the Unit 1 of Subject 1 ",
                            Answer = "input",
                            AnswerL = "ان پٹ",
                            QuestionId = 3
                        }
                    });
                }
                //save the changes to database
                context.SaveChanges();
                //QuestionMeta
                if (!context.QuestionMetas.Any())
                {
                    context.QuestionMetas.AddRange(new List<QuestionMeta>()
                    {
                        new QuestionMeta()
                        {
                            BoardName = BoardNames.Rawalpindi,
                            ExamName = Exam.Annual,
                            ExamYear = DateTime.Now,
                            Description = " this is a description",
                            Session = Session.Morning,
                            Keywords = "some keywords seperated by comma",
                            QuestionId   = 1
                        },
                        new QuestionMeta()
                        {
                            BoardName = BoardNames.Rawalpindi,
                            ExamName = Exam.Annual,
                            ExamYear = DateTime.Now,
                            Description = " this is a description",
                            Session = Session.Morning,
                            Keywords = "some keywords seperated by comma",
                            QuestionId   = 2
                        },
                        new QuestionMeta()
                        {
                            BoardName = BoardNames.Rawalpindi,
                            ExamName = Exam.Annual,
                            ExamYear = DateTime.Now,
                            Description = " this is a description",
                            Session = Session.Morning,
                            Keywords = "some keywords seperated by comma",
                            QuestionId   = 3
                        },
                    });
                }
                //save the changes to database
                context.SaveChanges();

                //schoolInfo
                if (!context.Plans.Any())
                {
                    context.Plans.AddRange(new List<Plan>()
                    {
                        new Plan()
                        {
                            // primary admin user need to be available 
                            Id = 1,
                            Name= "Free",
                            Price=0,
                            Description="Free plan for all users",
                        },
                        new Plan()
                        {
                            // primary admin user need to be available 
                            Id = 2,
                            Name= "Basic",
                            Price=500,
                            Description="Basic plan for all users. The user will only generate one exam paper per day.",
                        },
                        new Plan()
                        {
                            // primary admin user need to be available 
                            Id = 3,
                            Name= "Premium",
                            Price=5000,
                            Description="full plan for all users. The user will generate unlimited exam paper per day.",
                        }
                    });
                }

                //save the changes to database
                context.SaveChanges();
            }
        }

        public static async Task SeedUsersAndRollAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var rolManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // check weather the role exist in database, if not create a new one 
                if (!await rolManager.RoleExistsAsync(UserRoles.Admin))
                    await rolManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await rolManager.RoleExistsAsync(UserRoles.Teacher))
                    await rolManager.CreateAsync(new IdentityRole(UserRoles.Teacher));

                if (!await rolManager.RoleExistsAsync(UserRoles.Student))
                    await rolManager.CreateAsync(new IdentityRole(UserRoles.Student));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                


                // check if Application-user-Teacher exit 
                string appTeacherEmail = "Teacher@examsystem.com";
                var AppTeacher = await userManager.FindByEmailAsync(appTeacherEmail);
                if (AppTeacher == null)
                {
                    var newAppTeacher = new ApplicationUser()
                    {
                        FirstName = "Application",
                        LastName = "Teacher",
                        UserName = "app-Teacher",
                        Email = appTeacherEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppTeacher, "Teacher@12345");
                    var teacher = await userManager.FindByEmailAsync(appTeacherEmail);
                    if (teacher != null)
                    {
                        await userManager.AddToRoleAsync(newAppTeacher, UserRoles.Teacher);
                    }
                }

                // check if Application-user-Student exit 
                string appStudentEmail = "Student@examsystem.com";
                var AppStudent = await userManager.FindByEmailAsync(appStudentEmail);
                if (AppStudent == null)
                {
                    var newAppStudent = new ApplicationUser()
                    {
                        FirstName = "Application",
                        LastName = "Student",
                        UserName = "app-Student",
                        Email = appStudentEmail,
                        EmailConfirmed = true,
                        
                    };
                    await userManager.CreateAsync(newAppStudent, "Student@12345");
                    var Student = await userManager.FindByEmailAsync(appStudentEmail);
                    if (Student != null)
                    {
                        await userManager.AddToRoleAsync(newAppStudent, UserRoles.Student);
                    }
                }


                // check if ADMIN-user exit 
                string Adminemail = "admin@examsystem.com";
                var adminUser = await userManager.FindByEmailAsync(Adminemail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        UserName = "Admin-user",
                        Email = Adminemail,
                        EmailConfirmed = true,
                        
                    };
                    await userManager.CreateAsync(newAdminUser, "Admin@1234?");
                    //await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                    var admin = await userManager.FindByEmailAsync(Adminemail);
                    if (admin != null)
                    {
                        await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                    }
                }
            }

        }
    }
}

