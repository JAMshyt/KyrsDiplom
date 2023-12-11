using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.RInterface;

namespace recordBook.Repositories
{
	public class GroupRepositories : IGroup
	{
		private readonly Context _context;
		public GroupRepositories(Context context)
		{
			_context = context;
		}

		public async Task AddGroup(Group group)
		{
			await _context.Group.AddAsync(group);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteGroup(Group group)
		{
			_context.Remove(group);
			await _context.SaveChangesAsync();
		}

		public IQueryable<Group> GetAllGroup()
		{
			return _context.Group.AsQueryable();
		}

		public Task<Group?> GetGroup(Group group)
		{
			return GetAllGroup().Where(x => x.ID_Group == group.ID_Group).FirstOrDefaultAsync();
		}

		public Task<Group?> GetGroupbyID(int Id)
		{
			return GetAllGroup().Where(x => x.ID_Group == Id).FirstOrDefaultAsync();
		}

		public async Task UpdateGroup(Group group)
		{
			_context.Group.Update(group);
			await _context.SaveChangesAsync();
		}

	}
}
