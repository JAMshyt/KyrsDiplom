using recordBook.Models;

namespace recordBook.RInterface
{
	public interface ILogins
	{
		Task AddLogin(Logins kind_of_work);
		Task DeleteLogin(Logins kind_of_work);
		Task UpdateLogin(Logins kind_of_work);
		Logins? GetLoginbyID(int Id);
		IQueryable<Logins> GetAllLogins();
	}
}
