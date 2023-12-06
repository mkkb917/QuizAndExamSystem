using ExamSystem.Data.Base;
using ExamSystem.Data.Interface;
using ExamSystem.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using ExamSystem.Models;
using ExamSystem.Extensions;
using ExamSystem.Data.Static;

namespace ExamSystem.Data.Services
{
    public class SubjectService : EntityBaseRepository<Subject>, ISubjectService
    {
        private readonly AppDbContext _context;
        private readonly string webRootPath;
        public SubjectService(AppDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            webRootPath = webHostEnvironment.WebRootPath;
        }

        public async Task AddNewSubject(SubjectsVM data)
        {

            var newSubject = new Subject()
            {
                Code=data.Code,
                SubjectText = data.SubjectText,
                Image = data.Image,
                Status = data.Status,
                GradeId = data.GradeId,
                Description = data.Description,
                CreatedOn = DateTime.Now.Date,
                CreatedBy = data.user
            };

            //sve the data to the database
            await _context.Subjects.AddAsync(newSubject);
            await _context.SaveChangesAsync();
        }

        public Task DeleteSubject(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Subject>> GetAllActiveSubjectsById(int Id)
        {
            var responce = await _context.Subjects.Where(n => n.GradeId == Id&& n.Status == Status.Active).ToListAsync();
            return responce;
        }
        public async Task<Grade> GetGradeById(int Id)
        {
            var responce = await _context.Grades.Where(n => n.Id == Id).FirstOrDefaultAsync();
            return responce; 
        }
        public async Task<Subject> GetSubjectById(int Id)
        {
            var responce = await _context.Subjects.Where(n => n.Id == Id).FirstOrDefaultAsync();
            return responce;
        }

        public async Task<DropDownsListsVM> GetGradesList()
        {
            var responce = new DropDownsListsVM()
            {
                Grades = await _context.Grades.OrderBy(n => n.GradeText).ToListAsync(),
            };

            return responce;
        }

        public async Task<List<Topic>> GetAllTopicsById(int Id)
        {
            var responce = await _context.Topics.Where(s =>s.SubjectId == Id).ToListAsync();
            return responce;
        }

        public async Task UpdateSubject(int Id, SubjectsVM data)
        {
            var ObjSubject = await _context.Subjects.FirstOrDefaultAsync(n => n.Id == data.Id);
            if (ObjSubject != null)
            {
                ObjSubject.Code = data.Code;
                ObjSubject.SubjectText = data.SubjectText;
                ObjSubject.Image = data.Image;
                ObjSubject.Status = data.Status;
                ObjSubject.Description = data.Description;
                ObjSubject.UpdatedOn = DateTime.Now.Date;
                ObjSubject.UpdatedBy = data.user;

                //update the data to the database
                await _context.SaveChangesAsync();

            }
        }
        public string DeleteOldAndUploadNewFile(IFormFileCollection files, string? oldFile)
        {
            string Upload = webRootPath + WC.SubjectImagePath;

            // pass the file name, path and file to uploader
            string fileName = Path.GetFileNameWithoutExtension(files[0].FileName);
            //if the file already exsit then delete it before new upload
            if (oldFile != null)
            {
                //delete the old file
                var oldfile = Path.Combine(Upload, oldFile);
                //check if the file already exists then delete the old file
                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);

                }
            }
            // pass the new files object to funtion to save file and create thumbnail in directory
            var uploadImage = FileUploadAndConvert.UploadFileAndConvertToImage(files, Upload, fileName);
            return uploadImage;
        }
        public void DeleteFile(string fileName)
        {
            string Upload = webRootPath + WC.SubjectImagePath;

            //delete the old file
            var oldfile = Path.Combine(Upload, fileName);
            //check if the file already exists then delete the old file
            if (System.IO.File.Exists(oldfile))
            {
                System.IO.File.Delete(oldfile);

            }

        }

    }


}