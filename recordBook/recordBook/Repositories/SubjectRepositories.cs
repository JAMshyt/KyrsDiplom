using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.RInterface;

namespace recordBook.Repositories
{
	public class SubjectRepositories:ISubject
	{
		private readonly Context _context;
		public SubjectRepositories(Context context)
		{
			_context = context;
		}

		public async Task AddSubject(Subject subject)
		{
			await _context.Subject.AddAsync(subject);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteSubject(Subject subject)
		{
			_context.Remove(subject);
			await _context.SaveChangesAsync();
		}

		public IQueryable<Subject> GetAllSubject()
		{
			return _context.Subject.AsQueryable();
		}

		public Task<Subject?> GetSubject(Subject subject)
		{
			return GetAllSubject().Where(x => x.ID_Subject == subject.ID_Subject).FirstOrDefaultAsync();
		}

		public Task<Subject?> GetSubjectbyID(int Id)
		{
			return GetAllSubject().Where(x => x.ID_Subject == Id).FirstOrDefaultAsync();
		}

		public async Task UpdateSubject(Subject subject)
		{
			_context.Subject.Update(subject);
			await _context.SaveChangesAsync();
		}
	}
}
