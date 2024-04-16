using recordBook.Models;

namespace recordBook.RInterface
{
	public interface IRatingControl
	{
		Task AddRatingControl(RatingControl RatingControl);
		Task DeleteRatingControl(RatingControl RatingControl);
		Task UpdateRatingControl(RatingControl RatingControl);
		RatingControl? GetRatingControlbyID(int Id);
		IQueryable<RatingControl> GetAllRatingControl();
	}
}
