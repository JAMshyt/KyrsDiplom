using System.ComponentModel.DataAnnotations;

namespace recordBook.Models
{
	public class Subject
	{
		[Key]
		public int ID_Subject { get; set; }
		public string Name_subject { get; set; }
	}
}
