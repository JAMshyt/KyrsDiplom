using recordBook.Models;

namespace recordBook.RInterface
{
	public interface ICurator
	{
		Task AddCurator(Curator curator);
		Task DeleteCurator(Curator curator);
		Task UpdateCurator(Curator curator);
		Curator? GetCuratorbyID(int Id);
		IQueryable<Curator> GetAllCurator();
	}
}
