namespace recordBook.Models.ViewModels
{
	public class ExamsViewModel
	{
		public IEnumerable<Group> Groups { get; set; }
		public IEnumerable<Student> Students { get; set; }
		public IEnumerable<Group_Subject> Group_Subjects {  get; set; }
		public IEnumerable<Subject> Subjects { get; set; }
		public IEnumerable<Academic_performance> Academic_Performances { get; set; }
		public IEnumerable<Kind_of_work> Kind_of_works { get; set; }
		public IEnumerable<RatingControl>? RatingControls{ get; set; }
		public int selectedSemester {  get; set; }

		public Group selectedGroup { get; set; }
		public Subject selectedSubject { get; set; }
	}
}
