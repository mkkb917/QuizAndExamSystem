using ExamSystem.Data.Base;
using ExamSystem.Data.Static;
using ExamSystem.Models;

namespace ExamSystem.Data.Interface
{
    public interface IBookService : IEntityBaseRepository<Books>
    {
        Task<Books> GetBookById(int id);
        Task<List<Books>> GetAllBooksByUser(string Id);
        Task<List<Books>> GetAllBooksByStatus(Status status);
        Task<List<Books>> GetAllBooks();
        Task AddNewBook(Books data, IFormFileCollection files);
        Task UpdateBook(int Id, Books data, IFormFileCollection files);
        Task DeleteBook(int id);
    }
}
