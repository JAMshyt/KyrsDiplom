namespace recordBook.Models.ViewModels
{
	public class GroupSubjectViewModel
	{
		public Group group { get; set; }
		public IEnumerable<Subject> subjects { get; set; }
		public IEnumerable<Group_Subject> group_Subject { get; set; }
	}
}
