using recordBook.Models;

namespace recordBook.RInterface
{
	public interface IDepartment_worker
	{
		Task AddDepartment_worker(Department_worker department_worker);
		Task DeleteDepartment_worker(Department_worker department_worker);
		Task UpdateDepartment_worker(Department_worker department_worker);
		Department_worker? GetDepartment_workerbyID(int Id);
		IQueryable<Department_worker> GetAllDepartment_worker();
	}
}
