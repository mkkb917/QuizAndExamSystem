using ExamSystem.Data.Base;
using ExamSystem.Data.Interface;
using ExamSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamSystem.Data.Services
{
    // inherits the class from general Entity base repository by passing the Grade class entity
    public class GradeService:EntityBaseRepository<Grade>, IGradeService
    {
         
        private readonly AppDbContext _context;
        public GradeService(AppDbContext context):base(context)
        {
            _context = context;
        }

        

        public async Task<List<Subject>> GetSubjectById(int Id)
        {
            var responce = await _context.Subjects.Where(s => s.GradeId == Id).ToListAsync();
            return responce;
        }

        public async Task<List<Subject>?> GetSubjectByGrade(Grade item)
        {
            var responce = await _context.Subjects.Where(s => s.Grade == item).ToListAsync();
            return responce;
        }
        public async Task<bool>SearchGrade(string searchTerm)
        {
            var responce = await _context.Grades.Where(s=>s.GradeText==searchTerm).FirstOrDefaultAsync();
            if (responce!=null) return true;
            return false;
        }
    }
}
