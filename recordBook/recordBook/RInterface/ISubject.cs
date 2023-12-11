using recordBook.Models;

namespace recordBook.RInterface
{
	public interface ISubject
	{
		Task AddSubject(Subject subject);
		Task DeleteSubject(Subject subject);
		Task UpdateSubject(Subject subject);
		Task<Subject?> GetSubject(Subject subject);
		Task<Subject?> GetSubjectbyID(int Id); //get person by key
		IQueryable<Subject> GetAllSubject();
	}
}
