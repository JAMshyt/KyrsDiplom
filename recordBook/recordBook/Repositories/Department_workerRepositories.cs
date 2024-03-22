using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.RInterface;


namespace recordBook.Repositories
{
	public class Department_workerRepositories:IDepartment_worker
	{
		private readonly Context _context;
		public Department_workerRepositories(Context context)
		{
			_context = context;
		}

		public async Task AddDepartment_worker(Department_worker department_worker)
		{
			await _context.Department_worker.AddAsync(department_worker);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteDepartment_worker(Department_worker department_worker)
		{
			_context.Remove(department_worker);
			await _context.SaveChangesAsync();
		}

		public IQueryable<Department_worker> GetAllDepartment_worker()
		{
			return _context.Department_worker.AsQueryable();
		}

		public Department_worker? GetDepartment_workerbyID(int Id)
		{
			return GetAllDepartment_worker().Where(x => x.ID_Department_worker == Id).FirstOrDefaultAsync().Result;
		}

		public async Task UpdateDepartment_worker(Department_worker department_worker)
		{
			_context.Department_worker.Update(department_worker);
			await _context.SaveChangesAsync();
		}
	}
}
