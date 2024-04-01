using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.RInterface;

namespace recordBook.Repositories
{
	public class LoginsRepositories : ILogins
	{
		private readonly Context _context;
		public LoginsRepositories(Context context)
		{
			_context = context;
		}

		public async Task AddLogin(Logins Logins)
		{
			await _context.Logins.AddAsync(Logins);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteLogin(Logins Logins)
		{
			_context.Remove(Logins);
			await _context.SaveChangesAsync();
		}

		public IQueryable<Logins> GetAllLogins()
		{
			return _context.Logins.AsQueryable();
		}

		public Logins? GetLoginbyID(int Id)
		{
			return GetAllLogins().Where(x => x.ID_Login == Id).FirstOrDefaultAsync().Result;
		}

		public async Task UpdateLogin(Logins Logins)
		{
			_context.Logins.Update(Logins);
			await _context.SaveChangesAsync();
		}
	}
}
