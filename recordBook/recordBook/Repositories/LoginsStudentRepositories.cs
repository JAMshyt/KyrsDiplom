using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.RInterface;

namespace recordBook.Repositories
{
	public class LoginsStudentRepositories:ILoginsStudent
	{
		private readonly Context _context;
		public LoginsStudentRepositories(Context context)
		{
			_context = context;
		}

		public async Task AddLoginStudent(LoginsStudent Logins)
		{
			await _context.LoginsStudent.AddAsync(Logins);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteLoginStudent(LoginsStudent Logins)
		{
			_context.Remove(Logins);
			await _context.SaveChangesAsync();
		}

		public IQueryable<LoginsStudent> GetAllLoginsStudent()
		{
			return _context.LoginsStudent.AsQueryable();
		}

		public LoginsStudent? GetLoginStudentbyID(int Id)
		{
			return GetAllLoginsStudent().Where(x => x.Number_RecordBook == Id).FirstOrDefaultAsync().Result;
		}

		public async Task UpdateLoginStudent(LoginsStudent Logins)
		{
			_context.LoginsStudent.Update(Logins);
			await _context.SaveChangesAsync();
		}
	}
}
