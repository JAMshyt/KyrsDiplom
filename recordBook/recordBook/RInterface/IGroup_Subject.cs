using recordBook.Models;

namespace recordBook.RInterface
{
	public interface IGroup_Subject
	{
		Task AddGroup_Subject(Group_Subject group_subject);
		Task DeleteGroup_Subject(Group_Subject group_subject);
		Task UpdateGroup_Subject(Group_Subject group_subject);
		Task<Group_Subject?> GetGroup_Subject(Group_Subject group_subject);
		Task<Group_Subject?> GetGroup_SubjectbyID(int Id);
		IQueryable<Group_Subject> GetAllGroup_Subject();
	}
}
