namespace recordBook.Models.ViewModels
{
	public class AttendanceViewModel
	{
		public IEnumerable<Group> Groups { get; set; }
		public IEnumerable<Student> Students { get; set; }
		public IEnumerable<Group_Subject> Group_Subjects { get; set; }
		public IEnumerable<Subject> Subjects { get; set; }
		public IEnumerable<Attendance> Attendances { get; set; }

		public Group selectedGroup { get; set; }
		public Subject selectedSubject { get; set; }
	}
}
