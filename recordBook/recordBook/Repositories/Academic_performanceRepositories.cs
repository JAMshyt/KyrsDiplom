using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.RInterface;


namespace recordBook.Repositories
{
	public class Academic_performanceRepositories:IAcademic_performance
	{
		private readonly Context _context;
		public Academic_performanceRepositories(Context context)
		{
			_context = context;
		}

		public async Task AddAcademic_performance(Academic_performance academic_performance)
		{
			await _context.Academic_performance.AddAsync(academic_performance);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAcademic_performance(Academic_performance academic_performance)
		{
			_context.Remove(academic_performance);
			await _context.SaveChangesAsync();
		}

		public IQueryable<Academic_performance> GetAllAcademic_performance()
		{
			return _context.Academic_performance.AsQueryable();
		}

		public Task<Academic_performance?> GetAcademic_performance(Academic_performance academic_performance)
		{
			return GetAllAcademic_performance().Where(x => x.ID_Academic_performance == academic_performance.ID_Academic_performance).FirstOrDefaultAsync();
		}

		public Task<Academic_performance?> GetAcademic_performancebyID(int Id)
		{
			return GetAllAcademic_performance().Where(x => x.ID_Academic_performance == Id).FirstOrDefaultAsync();
		}

		public async Task UpdateAcademic_performance(Academic_performance academic_performance)
		{
			_context.Academic_performance.Update(academic_performance);
			await _context.SaveChangesAsync();
		}
	}
}
