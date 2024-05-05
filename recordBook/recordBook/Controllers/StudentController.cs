using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
		private readonly IAcademic_performance _academic_Performance;
		private readonly IKind_of_work _kind_Of_Work;
		private readonly ILoginsStudent _loginsStudent;


		private readonly ILogins _user;

		public StudentController(ILogger<StudentController> logger, IStudent student,
			IGroup group, ISubject subject, IGroup_Subject group_subject, ILogins user,
			ILogins logins, IAcademic_performance academic_Performance, IKind_of_work kind_Of_Work,
			ILoginsStudent loginsStudent
			)
		{
			_logger = logger;
			_student = student;
			_group = group;
			_subject = subject;
			_group_subject = group_subject;
			_logins = logins;
			_academic_Performance = academic_Performance;
			_kind_Of_Work = kind_Of_Work;
			_loginsStudent = loginsStudent;
		}


		#region Get таблиц
		public List<Student> GetStudents()
		{
			var students = _student.GetAllStudent().ToList();
			return students;
		}

		public List<Subject> GetSubject()
		{
			var subj = _subject.GetAllSubject().ToList();
			return subj;
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

		public List<LoginsStudent> GetLoginsStudents()
		{
			var logS = _loginsStudent.GetAllLoginsStudent().ToList();
			return logS;
		}

		public List<Group_Subject> GetGroupSubject()
		{
			var gs = _group_subject.GetAllGroup_Subject().ToList();
			return gs;
		}

		public List<Academic_performance> GetAcademicPerformance()
		{
			var ap = _academic_Performance.GetAllAcademic_performance().ToList();
			return ap;
		}

		public List<Kind_of_work> GetKindOfWork()
		{
			var kow = _kind_Of_Work.GetAllKind_of_work().ToList();
			return kow;
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
			var model = new GroupsStudentsViewModel
			{
				Groups = GetGroups(),
				Students = GetStudents(),
				subjects = GetSubject(),
				academic_Performance = GetAcademicPerformance(),
				kind_Of_Works = GetKindOfWork(),
			};

			if (User.IsInRole("Adm"))
			{
				if (selectedGroup > 0)
				{
					var groupById = _group.GetGroupbyID(selectedGroup);

					model.selectedGroup = groupById;
					model.group_Subject = GetGroupSubject().Where(q => q.ID_Group == groupById.ID_Group);

					return View(model);
				}
				else
				{

					model.selectedGroup = GetGroups().FirstOrDefault();
					model.group_Subject = GetGroupSubject().Where(q => q.ID_Group == GetGroups().FirstOrDefault().ID_Group);
					return View(model);
				}
			}
			else if(User.IsInRole("Student"))
			{

				model.selectedGroup = GetGroups().FirstOrDefault(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
				model.group_Subject = GetGroupSubject().Where(q => q.ID_Group == GetGroups().FirstOrDefault(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value)).ID_Group);

				return View(model);
			}
			else
			{
				var groupClaims = User.FindAll(ClaimTypes.GroupSid).Select(claim => Convert.ToInt32(claim.Value)).ToList();
				var selectedGroups = GetGroups().Where(group => groupClaims.Contains(group.ID_Group));
				model.Groups = selectedGroups;
				model.group_Subject = GetGroupSubject().Where(subject =>subject.ID_Group == selectedGroups.FirstOrDefault().ID_Group);
				model.selectedGroup = selectedGroups.FirstOrDefault();

				if (selectedGroup > 0)
				{
					var groupById = _group.GetGroupbyID(selectedGroup);
					model.group_Subject = GetGroupSubject().Where(q => q.ID_Group == groupById.ID_Group);
					model.selectedGroup = groupById;

					return View(model);
				}

				return View(model);
			}
		}

		public class IdAndDate
		{
			public int idsubj { get; set; }
			public int group { get; set; }
			public DateTime newDate { get; set; }

		}
		[HttpPost]
		[Route("/Student/ChangeDate/")]
		[Consumes("application/json")]
		public async Task<IActionResult> ChangeDate([FromBody] IdAndDate request)
		{
			List<int> idStudent = GetStudents().Where(q => q.ID_Group == request.group).Select(q => q.ID_Student).ToList();
			foreach (var r in idStudent)
			{
				Academic_performance oldAcademPerf = GetAcademicPerformance().FirstOrDefault(z => z.ID_Subject == request.idsubj && z.ID_Student == r);
				oldAcademPerf.Date = request.newDate;
				await _academic_Performance.UpdateAcademic_performance(oldAcademPerf);
			}
			var str = request.idsubj + " " + request.newDate;
			return Json(str);
		}


		public class StudentId
		{
			public int Id { get; set; }
		}
		/// <summary>
		/// удаляет студента
		/// </summary>
		/// <param name="studId">id студента</param>
		/// <returns>возвращает на сайт</returns>
		[HttpPost]
		[Route("Student/DropStudent/")]
		[Consumes("application/json")]
		public async Task<IActionResult> DropStudent([FromBody] StudentId studId)
		{
			var stu = _student.GetStudentbyID(studId.Id);
			var log = _loginsStudent.GetAllLoginsStudent().FirstOrDefault(q=>q.Number_RecordBook == stu.NumberOfBook);
			if (stu != null)
			{
				await _student.DeleteStudent(stu);
				await _loginsStudent.DeleteLoginStudent(log);
			}
			return Json(stu);
		}


		public class StudentIdNewGroup
		{
			public int id { get; set; }
			public int newGroup { get; set; }
		}
		/// <summary>
		/// Меняет группу у студента
		/// </summary>
		/// <param name="id">id студента</param>
		/// <returns>обратно на сайт
		/// </returns>
		[HttpPost]
		[Route("Student/ChangeStudentGroup/")]
		[Consumes("application/json")]
		public async Task<IActionResult> ChangeStudentGroup([FromBody] StudentIdNewGroup newGroup)
		{
			var stu = _student.GetStudentbyID(newGroup.id);
			if (stu != null)
			{
				stu.ID_Group = newGroup.newGroup;
				await _student.UpdateStudent(stu);
			}
			return Json(stu);
		}

		public class newExam
		{
			public int idSubj { get; set; }
			public int idWork { get; set; }
			public int idGroup { get; set; }
		}
		/// <summary>
		/// Добавляет новый экзамен
		/// </summary>
		/// <param name="newGroup"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("Student/addExam/")]
		[Consumes("application/json")]
		public async Task<IActionResult> addExam([FromBody] newExam exam)
		{

			List<Student> StudentsInGroup = GetStudents().Where(q => q.ID_Group == exam.idGroup).ToList();
			foreach (var r in StudentsInGroup)
			{
			var perf = new Academic_performance()
			{
				ID_Subject = exam.idSubj,
				ID_Kind_of_work = exam.idWork,
				Grade = "Нет оценки",
				Date = null,
				ID_Student = r.ID_Student
			};
				await _academic_Performance.AddAcademic_performance(perf);
			}

			return Json("");
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
			var nullLogin = GetLoginsStudents().FirstOrDefault(q => q.Login == addStu.Login);
			var nullEmail = GetLoginsStudents().FirstOrDefault(q => q.Email == addStu.Email);

			string book = addStu.NumberBook.ToString();

			if (nullLogin == null && nullEmail == null)
			{
				if (ModelState.IsValid && book.Length == 7)
				{
					string CreateSalt()
					{
						byte[] salt = new byte[16];
						using (var rng = RandomNumberGenerator.Create())
						{
							rng.GetBytes(salt);
						}
						return Convert.ToBase64String(salt).Substring(0, 16);
					}

					string HashPassword(string password, string salt)
					{
						byte[] saltBytes = Convert.FromBase64String(salt);
						byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

						byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
						Array.Copy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
						Array.Copy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

						using (var sha512 = SHA512.Create())
						{
							byte[] hashedBytes = sha512.ComputeHash(combinedBytes);
							return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
						}
					}

					string salt = CreateSalt();
					string hashedPassword = HashPassword(addStu.Password, salt);

					var login = new LoginsStudent()
					{
						Number_RecordBook = addStu.NumberBook,
						Login = addStu.Login,
						Password = hashedPassword,
						Email = addStu.Email,
						Salt = salt,
						Phone = Convert.ToDecimal(addStu.Phone)
					};
					await _loginsStudent.AddLoginStudent(login);

					var idNewLogin = GetLoginsStudents().FirstOrDefault(q => q.Login == addStu.Login);
					var addStudent = new Student() { Surname = addStu.Surname, Name = addStu.Name, Patronymic = addStu.Patronymic, ID_Group = addStu.ID_Group, NumberOfBook = idNewLogin.Number_RecordBook};
					await _student.AddStudent(addStudent);
					var model = new AddStudentViewModel {NumberBook = addStu.NumberBook, Surname = addStu.Surname, Name = addStu.Name, Patronymic = addStu.Patronymic, ID_Group = addStu.ID_Group, Groups = GetGroups(), studentAdded = true, loginUnique = true, EmailUnique = true };
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
				var model2 = new AddStudentViewModel { Groups = GetGroups(), studentAdded = false, EmailUnique = true, loginUnique = true };
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
