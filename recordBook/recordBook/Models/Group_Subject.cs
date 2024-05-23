using System.ComponentModel.DataAnnotations;

namespace recordBook.Models
{
	public class Group_Subject
	{
		[Key]
		public int ID_Group_Subject { get; set; }
		public int ID_Group { get; set; }
		public int ID_Subject { get; set; }

		public int? Semester {  get; set; }

	}
}
