using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.RInterface;


namespace recordBook.Repositories
{
	public class AttendanceRepositories: IAttendance
	{
		private readonly Context _context;
		public AttendanceRepositories(Context context)
		{
			_context = context;
		}

		public async Task AddAttendance(Attendance Attendance)
		{
			await _context.Attendance.AddAsync(Attendance);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAttendance(Attendance Attendance)
		{
			_context.Remove(Attendance);
			await _context.SaveChangesAsync();
		}

		public IQueryable<Attendance> GetAllAttendance()
		{
			return _context.Attendance.AsQueryable();
		}

		public Task<Attendance?> GetAttendance(Attendance Attendance)
		{
			return GetAllAttendance().Where(x => x.ID_Attendance == Attendance.ID_Attendance).FirstOrDefaultAsync();
		}

		public Task<Attendance?> GetAttendancebyID(int Id)
		{
			return GetAllAttendance().Where(x => x.ID_Attendance == Id).FirstOrDefaultAsync();
		}

		public async Task UpdateAttendance(Attendance Attendance)
		{
			_context.Attendance.Update(Attendance);
			await _context.SaveChangesAsync();
		}
	}
}
