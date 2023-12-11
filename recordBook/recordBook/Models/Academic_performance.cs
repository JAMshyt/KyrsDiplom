using System.ComponentModel.DataAnnotations;

namespace recordBook.Models
{
	public class Academic_performance
	{
		[Key]
		public int ID_Academic_performance { get; set; }
		public int ID_Subject { get; set; }
		public int ID_Student { get; set; }
		public int ID_Kind_of_work { get; set; }
		public DateTime Date { get; set; }
		public string Grade { get; set; }
	}
}
