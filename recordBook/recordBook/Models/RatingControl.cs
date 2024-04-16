using System.ComponentModel.DataAnnotations;

namespace recordBook.Models
{
	public class RatingControl
	{
		[Key]
		public int ID_RatingControl {  get; set; }
		public int ID_Student { get; set; }
		public int ID_Subject { get; set; }
		public int Semester { get; set; }
		public int Points { get; set; }
		public int RatingNumber{ get; set;}
	}
}
