using System.ComponentModel.DataAnnotations;

namespace recordBook.Models
{
	public class Student
	{
		[Key]
		public int ID_Student { get; set; }
		public string Surname { get; set; }
		public string Name { get; set; }
		public string Patronymic { get; set; }
		public int ID_Group { get; set; }

	}
}
