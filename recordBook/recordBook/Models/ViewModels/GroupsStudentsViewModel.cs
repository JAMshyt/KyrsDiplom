namespace recordBook.Models.ViewModels
{
	public class GroupsStudentsViewModel
	{
		
		public IEnumerable<Group> Groups { get; set; }
		public IEnumerable<Student> Students { get;set; }
		public Group selectedGroup { get; set; }
		public IEnumerable<Group_Subject> group_Subject { get; set; }
		public IEnumerable<Subject> subjects { get; set; }
		public IEnumerable<Academic_performance> academic_Performance { get; set; }
		public IEnumerable<Kind_of_work> kind_Of_Works { get; set; }
	}
}
