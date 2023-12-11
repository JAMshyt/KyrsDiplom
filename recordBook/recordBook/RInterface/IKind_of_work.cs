using recordBook.Models;

namespace recordBook.RInterface
{
	public interface IKind_of_work
	{
		Task AddKind_of_work(Kind_of_work kind_of_work);
		Task DeleteKind_of_work(Kind_of_work kind_of_work);
		Task UpdateKind_of_work(Kind_of_work kind_of_work);
		Task<Kind_of_work?> GetKind_of_work(Kind_of_work kind_of_work);
		Task<Kind_of_work?> GetKind_of_workbyID(int Id);
		IQueryable<Kind_of_work> GetAllKind_of_work();
	}
}
