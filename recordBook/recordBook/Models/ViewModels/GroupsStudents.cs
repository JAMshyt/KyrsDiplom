namespace recordBook.Models.ViewModels
{
	public class GroupsStudents
	{
		public IEnumerable<Group> Groups { get; set; }
		public IEnumerable<Student> Students { get;set; }
		public Group selectedGroup { get; set; }
	}
}
