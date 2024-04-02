namespace recordBook.Models.ViewModels
{
	public class GroupsStudentsViewModel
	{
		
		public IEnumerable<Group> Groups { get; set; }
		public IEnumerable<Student> Students { get;set; }
		public Group selectedGroup { get; set; }
	}
}
