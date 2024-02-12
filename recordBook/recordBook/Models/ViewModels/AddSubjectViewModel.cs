using System.ComponentModel.DataAnnotations;

namespace recordBook.Models.ViewModels
{
	public class AddSubjectViewModel
	{
		[Required(ErrorMessage = "Введите название")]
		public string NameSubject { get; set; }
		public IEnumerable<Group> Groups { get; set; }

		public Group selectedGroup { get; set; }
	}
}
