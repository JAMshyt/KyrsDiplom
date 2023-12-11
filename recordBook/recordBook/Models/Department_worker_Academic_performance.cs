using System.ComponentModel.DataAnnotations;

namespace recordBook.Models
{
	public class Department_worker_Academic_performance
	{
		[Key]
		public int ID_Department_worker_Academic_performance { get; set; }
		public int ID_Department_worker { get; set; }
		public int ID_Academic_performance { get; set; }
	}
}
