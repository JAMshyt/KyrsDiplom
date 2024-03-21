using System.ComponentModel.DataAnnotations;

namespace recordBook.Models.ViewModels
{
	public class AddSubjectViewModel
	{
		[Required(ErrorMessage = "Введите название")]
		public string NameSubject { get; set; }
		public IEnumerable<Group>? Groups { get; set; }
		public bool subjectAdded { get; set; }
		[MinLengthAttribute(1)]
		public int[] selectedGroups { get; set; }

	}
}
