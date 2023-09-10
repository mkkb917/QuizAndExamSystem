using ExamSystem.Data.Base;
using ExamSystem.Data.Interface;
using ExamSystem.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using ExamSystem.Models;

namespace ExamSystem.Data.Services
{
    public class SubjectService : EntityBaseRepository<Subject>, ISubjectService
    {
        private readonly AppDbContext _context;
        public SubjectService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewSubject(SubjectsVM data)
        {

            var newSubject = new Subject()
            {
                Code=data.Code,
                SubjectText = data.SubjectText,
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

        public async Task<List<Subject>> GetAllSubjectsById(int Id)
        {
            var responce = await _context.Subjects.Where(n => n.GradeId == Id).ToListAsync();
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
                ObjSubject.Status = data.Status;
                ObjSubject.Description = data.Description;
                ObjSubject.UpdatedOn = DateTime.Now.Date;
                ObjSubject.UpdatedBy = data.user;

                //update the data to the database
                await _context.SaveChangesAsync();

            }
        }

    }


}