using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using recordBook.Models;
using recordBook.Models.ViewModels;
using recordBook.RInterface;

namespace recordBook.Controllers
{
	public class StudentController : Controller
	{
		private readonly ILogger<StudentController> _logger;

		private readonly IStudent _student;
		private readonly IGroup _group;
		private readonly ISubject _subject;
		private readonly IGroup_Subject _group_subject;
		private readonly ILogins _logins;

		private readonly ILogins _user;

		public StudentController(ILogger<StudentController> logger, IStudent student,
			IGroup group, ISubject subject, IGroup_Subject group_subject, ILogins user,
			ILogins logins
			)
		{
			_logger = logger;
			_student = student;
			_group = group;
			_subject = subject;
			_group_subject = group_subject;
			_logins = logins;
		}


		#region Get таблиц
		public List<Student> GetStudents()
		{
			var students = _student.GetAllStudent().ToList();
			return students;
		}

		public List<Group> GetGroups()
		{
			var group = _group.GetAllGroup().ToList();
			return group;
		}
		public List<Logins> GetLogins()
		{
			var logins = _logins.GetAllLogins().ToList();
			return logins;
		}
		#endregion


		/// <summary>
		/// выводит всех студентов по группам
		/// </summary>
		/// <param name="selectedGroup">id выбранной группы</param>
		/// <returns>модел с всеми учениками выбранной группы</returns>
		public async Task<IActionResult> ShowStudents(int selectedGroup)
		{
			ViewData["User"] = User.FindFirst(ClaimTypes.Surname)?.Value + " " + User.FindFirst(ClaimTypes.Name)?.Value;
			if (selectedGroup > 0)
			{
				var groupById = _group.GetGroupbyID(selectedGroup);
				var model = new GroupsStudentsViewModel { Groups = GetGroups(), Students = GetStudents(), selectedGroup = groupById };
				return View(model);
			}
			else
			{
				var model = new GroupsStudentsViewModel { Groups = GetGroups(), Students = GetStudents(), selectedGroup = GetGroups().FirstOrDefault() };
				return View(model);
			}
		}


		/// <summary>
		/// удаляет студента
		/// </summary>
		/// <param name="Id">id студента</param>
		/// <returns>возвращает обратно на сайт но не обновляет содержимое, чтобы студент убрался надо обновить страницу</returns>
		[HttpGet]
		[Route("Student/DropStudent/{Id:int}")]
		public async Task<IActionResult> DropStudent(int Id)
		{
			var stu = _student.GetStudentbyID(Id);
			if (stu != null)
			{
				await _student.DeleteStudent(stu);
			}
			return RedirectToAction(nameof(ShowStudents));
		}


		/// <summary>
		/// страница добавления студента
		/// </summary>
		/// <returns></returns>
		public ViewResult AddStudent() /*async Task<IActionResult>*/
		{
			ViewData["User"] = User.FindFirst(ClaimTypes.Surname)?.Value + " " + User.FindFirst(ClaimTypes.Name)?.Value;
			var model2 = new AddStudentViewModel { Groups = GetGroups(), ID_Group = GetGroups().FirstOrDefault().ID_Group, studentAdded = false, loginUnique = true, EmailUnique = true };
			return View(model2);
		}

		/// <summary>
		/// добавляет студента, проверяет заполненность нужных полей
		/// </summary>
		/// <param name="addStu">модель студента</param>
		/// <returns>возвращает обратно на сайт с оповещением о результате добваления</returns>
		[HttpPost]
		public async Task<IActionResult> AddStudent(AddStudentViewModel addStu)
		{
			var nullLogin = GetLogins().FirstOrDefault(q => q.Login == addStu.Login);
			var nullEmail = GetLogins().FirstOrDefault(q => q.Email == addStu.Email);
			if (nullLogin == null && nullEmail == null)
			{
				if (ModelState.IsValid)
				{
					var login = new Logins() { Login = addStu.Login, Password = addStu.Password, Email = addStu.Email };
					await _logins.AddLogin(login);

					var idNewLogin = GetLogins().FirstOrDefault(q => q.Login == addStu.Login);
					var addStudent = new Student() { Surname = addStu.Surname, Name = addStu.Name, Patronymic = addStu.Patronymic, ID_Group = addStu.ID_Group, ID_Login = idNewLogin.ID_Login };
					await _student.AddStudent(addStudent);
					var model = new AddStudentViewModel { Surname = addStu.Surname, Name = addStu.Name, Patronymic = addStu.Patronymic, ID_Group = addStu.ID_Group, Groups = GetGroups(), studentAdded = true, loginUnique = true, EmailUnique = true };
					return View(model);
				}
				else
				{
					var model2 = new AddStudentViewModel { Groups = GetGroups(), studentAdded = false, loginUnique = true, EmailUnique = true };
					return View(model2);
				}
			}
			else
			{
				var model2 = new AddStudentViewModel { Groups = GetGroups(), EmailUnique = true, loginUnique = true };
				if (nullLogin != null && nullEmail != null)
				{
					model2.EmailUnique = false;
					model2.loginUnique = false;
				}
				else
				{
					if (nullLogin == null) { model2.EmailUnique = false; }
					else { model2.loginUnique = false; };
				};
				return View(model2);
			}
		}
	}
}
