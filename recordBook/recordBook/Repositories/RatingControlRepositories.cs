using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.RInterface;

namespace recordBook.Repositories
{
	public class RatingControlRepositories : IRatingControl
	{
		private readonly Context _context;
		public RatingControlRepositories(Context context)
		{
			_context = context;
		}

		public async Task AddRatingControl(RatingControl RatingControl)
		{
			await _context.RatingControl.AddAsync(RatingControl);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteRatingControl(RatingControl RatingControl)
		{
			_context.Remove(RatingControl);
			await _context.SaveChangesAsync();
		}

		public IQueryable<RatingControl> GetAllRatingControl()
		{
			return _context.RatingControl.AsQueryable();
		}


		public RatingControl? GetRatingControlbyID(int Id)
		{
			return GetAllRatingControl().Where(x => x.ID_RatingControl == Id).FirstOrDefaultAsync().Result;
		}

		public async Task UpdateRatingControl(RatingControl RatingControl)
		{
			_context.RatingControl.Update(RatingControl);
			await _context.SaveChangesAsync();
		}
	}
}
