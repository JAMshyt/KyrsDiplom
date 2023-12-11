using System.ComponentModel.DataAnnotations;


namespace recordBook.Models
{
	public class Attendance
	{
		[Key]
		public int ID_Attendance { get; set; }
		public int ID_Subject { get; set; }
		public int ID_Student { get; set; }
		public DateTime Date_precense { get; set; }
		public bool Precense { get; set; }
		public int ID_Department_worker { get; set; }
	}
}
