using System.ComponentModel.DataAnnotations;


namespace recordBook.Models
{
	public class Department_worker
	{
		[Key]
		public int ID_Department_worker { get; set; }
		public string Surname { get; set; }
		public string Name { get; set; }
		public string Patronymic { get; set; }
		public int ID_Login { get; set; }

		public string Institute_title { get; set; }
		public string Job_title { get; set; }
		public byte[]? Photo { get; set; }
	}
}
