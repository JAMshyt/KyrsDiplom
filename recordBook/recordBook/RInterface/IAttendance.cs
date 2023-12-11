using recordBook.Models;

namespace recordBook.RInterface
{
	public interface IAttendance
	{
		Task AddAttendance(Attendance attendance);
		Task DeleteAttendance(Attendance attendance);
		Task UpdateAttendance(Attendance attendance);
		Task<Attendance?> GetAttendance(Attendance attendance);
		Task<Attendance?> GetAttendancebyID(int Id);
		IQueryable<Attendance> GetAllAttendance();
	}
}
