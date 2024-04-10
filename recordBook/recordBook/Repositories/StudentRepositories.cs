using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.RInterface;

namespace recordBook.Repositories
{
	public class StudentRepositories : IStudent
	{
		private readonly Context _context;
		public StudentRepositories(Context context)
		{
			_context = context;
		}

		public async Task AddStudent(Student student)
		{
			await _context.Student.AddAsync(student);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteStudent(Student student)
		{
			_context.Remove(student);
			await _context.SaveChangesAsync();
		}

		public IQueryable<Student> GetAllStudent()
		{
			return _context.Student.AsQueryable();
		}


		public Student? GetStudentbyID(int Id)
		{
			return GetAllStudent().Where(x => x.ID_Student == Id).FirstOrDefaultAsync().Result;
		}

		public async Task UpdateStudent(Student student)
		{
			_context.Student.Update(student);
			await _context.SaveChangesAsync();
		}
	}
}

