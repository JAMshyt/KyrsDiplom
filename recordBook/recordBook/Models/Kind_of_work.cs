using System.ComponentModel.DataAnnotations;

namespace recordBook.Models
{
	public class Kind_of_work
	{
		[Key]
		public int ID_Kind_of_work { get; set; }
		public string Title_of_kind { get; set; }

	}
}
