using recordBook.Models;

namespace recordBook.RInterface
{
	public interface IStudent
	{
		Task AddStudent(Student student);
		Task DeleteStudent(Student student);
		Task UpdateStudentl(Student student);
		Task<Student?> GetStudent(Student student);
		Task<Student?> GetStudentbyID(int Id); 
		IQueryable<Student> GetAllStudent();
	}
}
