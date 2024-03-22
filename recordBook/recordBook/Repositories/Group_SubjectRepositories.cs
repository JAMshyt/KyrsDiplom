using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.RInterface;

namespace recordBook.Repositories
{
	public class Group_SubjectRepositories:IGroup_Subject
	{
		private readonly Context _context;
		public Group_SubjectRepositories(Context context)
		{
			_context = context;
		}

		public async Task AddGroup_Subject(Group_Subject group_subject)
		{
			await _context.Group_Subject.AddAsync(group_subject);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteGroup_Subject(Group_Subject group_subject)
		{
			_context.Remove(group_subject);
			await _context.SaveChangesAsync();
		}

		public IQueryable<Group_Subject> GetAllGroup_Subject()
		{
			return _context.Group_Subject.AsQueryable();
		}

        public IQueryable<Group_Subject?> GetGroup_SubjectbyGroupID(int GroupId)
        {
            return GetAllGroup_Subject().Where(x => x.ID_Group == GroupId).AsQueryable();
        }

		public Group_Subject? GetGroup_SubjectbyID(int Id)
		{
			return GetAllGroup_Subject().Where(x => x.ID_Group_Subject == Id).FirstOrDefaultAsync().Result;
		}

        public async Task UpdateGroup_Subject(Group_Subject group_subject)
		{
			_context.Group_Subject.Update(group_subject);
			await _context.SaveChangesAsync();
		}
	}
}
