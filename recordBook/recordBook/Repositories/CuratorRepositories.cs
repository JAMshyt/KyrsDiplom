using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.RInterface;

namespace recordBook.Repositories
{
	public class CuratorRepositories : ICurator
	{
		private readonly Context _context;
		public CuratorRepositories(Context context)
		{
			_context = context;
		}

		public async Task AddCurator(Curator Curator)
		{
			await _context.Curator.AddAsync(Curator);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCurator(Curator Curator)
		{
			_context.Remove(Curator);
			await _context.SaveChangesAsync();
		}

		public IQueryable<Curator> GetAllCurator()
		{
			return _context.Curator.AsQueryable();
		}


		public Curator? GetCuratorbyID(int Id)
		{
			return GetAllCurator().Where(x => x.ID_Curator == Id).FirstOrDefaultAsync().Result;
		}

		public async Task UpdateCurator(Curator Curator)
		{
			_context.Curator.Update(Curator);
			await _context.SaveChangesAsync();
		}
	}
}

