using recordBook.Models;

namespace recordBook.RInterface
{
	public interface ILogins
	{
		Task AddLogin(Logins log);
		Task DeleteLogin(Logins log);
		Task UpdateLogin(Logins log);
		Logins? GetLoginbyID(int Id);
		IQueryable<Logins> GetAllLogins();
	}
}
