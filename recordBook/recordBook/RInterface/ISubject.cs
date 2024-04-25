using recordBook.Models;

namespace recordBook.RInterface
{
	public interface ISubject
	{
		Task AddSubject(Subject subject);
		Task DeleteSubject(Subject subject);
		Task UpdateSubject(Subject subject);
		Subject? GetSubjectbyID(int? Id); 
		IQueryable<Subject> GetAllSubject();
	}
}
