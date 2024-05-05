using recordBook.Models;
namespace recordBook.RInterface
{
	public interface ILoginsStudent
	{
		Task AddLoginStudent(LoginsStudent log);
		Task DeleteLoginStudent(LoginsStudent log);
		Task UpdateLoginStudent(LoginsStudent log);
		LoginsStudent? GetLoginStudentbyID(int Id);
		IQueryable<LoginsStudent> GetAllLoginsStudent();
	}
}
