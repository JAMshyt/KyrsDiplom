using recordBook.Models;

namespace recordBook.RInterface
{
	public interface IGroup
	{
		Task AddGroup(Group group);
		Task DeleteGroup(Group group);
		Task UpdateGroup(Group group);
		Group? GetGroupbyID(int Id); 
		IQueryable<Group> GetAllGroup();
	}
}
