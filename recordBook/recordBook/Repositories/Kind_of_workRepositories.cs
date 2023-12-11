using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.RInterface;

namespace recordBook.Repositories
{
	public class Kind_of_workRepositories:IKind_of_work
	{
		private readonly Context _context;
		public Kind_of_workRepositories(Context context)
		{
			_context = context;
		}

		public async Task AddKind_of_work(Kind_of_work kind_of_work)
		{
			await _context.Kind_of_work.AddAsync(kind_of_work);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteKind_of_work(Kind_of_work kind_of_work)
		{
			_context.Remove(kind_of_work);
			await _context.SaveChangesAsync();
		}

		public IQueryable<Kind_of_work> GetAllKind_of_work()
		{
			return _context.Kind_of_work.AsQueryable();
		}

		public Task<Kind_of_work?> GetKind_of_work(Kind_of_work kind_of_work)
		{
			return GetAllKind_of_work().Where(x => x.ID_Kind_of_work == kind_of_work.ID_Kind_of_work).FirstOrDefaultAsync();
		}

		public Task<Kind_of_work?> GetKind_of_workbyID(int Id)
		{
			return GetAllKind_of_work().Where(x => x.ID_Kind_of_work == Id).FirstOrDefaultAsync();
		}

		public async Task UpdateKind_of_work(Kind_of_work kind_of_work)
		{
			_context.Kind_of_work.Update(kind_of_work);
			await _context.SaveChangesAsync();
		}
	}
}
