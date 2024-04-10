using recordBook.Models;

namespace recordBook.RInterface
{
	public interface IStudent
	{
		Task AddStudent(Student student);
		Task DeleteStudent(Student student);
		Task UpdateStudent(Student student);
		Student? GetStudentbyID(int Id); 
		IQueryable<Student> GetAllStudent();
	}
}
