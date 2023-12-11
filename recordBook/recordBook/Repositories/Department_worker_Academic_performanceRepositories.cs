using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.RInterface;

namespace recordBook.Repositories
{
	public class Department_worker_Academic_performanceRepositories:IDepartment_worker_Academic_performance
	{
		private readonly Context _context;
		public Department_worker_Academic_performanceRepositories(Context context)
		{
			_context = context;
		}	

		public async Task AddDepartment_worker_Academic_performance(Department_worker_Academic_performance department_worker_academic_performance)
		{
			await _context.Department_worker_Academic_performance.AddAsync(department_worker_academic_performance);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteDepartment_worker_Academic_performance(Department_worker_Academic_performance department_worker_academic_performance)
		{
			_context.Remove(department_worker_academic_performance);
			await _context.SaveChangesAsync();
		}

		public IQueryable<Department_worker_Academic_performance> GetAllDepartment_worker_Academic_performance()
		{
			return _context.Department_worker_Academic_performance.AsQueryable();
		}

		public Task<Department_worker_Academic_performance?> GetDepartment_worker_Academic_performance(Department_worker_Academic_performance department_worker_academic_performance)
		{
			return GetAllDepartment_worker_Academic_performance().Where(x => x.ID_Department_worker_Academic_performance == department_worker_academic_performance.ID_Department_worker_Academic_performance).FirstOrDefaultAsync();
		}

		public Task<Department_worker_Academic_performance?> GetDepartment_worker_Academic_performancebyID(int Id)
		{
			return GetAllDepartment_worker_Academic_performance().Where(x => x.ID_Department_worker_Academic_performance == Id).FirstOrDefaultAsync();
		}

		public async Task UpdateDepartment_worker_Academic_performance(Department_worker_Academic_performance department_worker_academic_performance)
		{
			_context.Department_worker_Academic_performance.Update(department_worker_academic_performance);
			await _context.SaveChangesAsync();
		}
	}
}
