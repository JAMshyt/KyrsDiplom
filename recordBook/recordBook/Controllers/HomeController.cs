using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.Models.ViewModels;
using recordBook.RInterface;

namespace recordBook.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private readonly IStudent _student;
		private readonly IGroup _group;
		private readonly ISubject _subject;
		private readonly IKind_of_work _kind_of_work;
		private readonly IDepartment_worker _department_worker;
		private readonly IAcademic_performance _academic_performance;
		private readonly IAttendance _attedance;
		private readonly IDepartment_worker_Academic_performance _department_worker_academic_performance;
		private readonly IGroup_Subject _group_subject;

		public HomeController(ILogger<HomeController> logger, IStudent student,
			IGroup group, ISubject subject, IKind_of_work kind_wf_work,
			IDepartment_worker department_worker, IAcademic_performance academic_performance,
			IAttendance attendance, IDepartment_worker_Academic_performance department_worker_academic_performance,
			IGroup_Subject group_subject
			)
		{
			_logger = logger;
			_student = student;
			_group = group;
			_subject = subject;
			_kind_of_work = kind_wf_work;
			_department_worker = department_worker;
			_academic_performance = academic_performance;
			_attedance = attendance;
			_department_worker_academic_performance = department_worker_academic_performance;
			_group_subject = group_subject;
		}

		//Get таблиц
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

		public List<Subject> GetSubjects()
		{
			var subjects = _subject.GetAllSubject().ToList();
			return subjects;
		}

		public List<Kind_of_work> GetKind_of_works()
		{
			var kind_of_work = _kind_of_work.GetAllKind_of_work().ToList();
			return kind_of_work;
		}

		public List<Department_worker> GetDepartment_worker()
		{
			var dep_wor = _department_worker.GetAllDepartment_worker().ToList();
			return dep_wor;
		}

		public List<Academic_performance> GetAcademic_performance()
		{
			var acad_perf = _academic_performance.GetAllAcademic_performance().ToList();
			return acad_perf;
		}

		public List<Attendance> GetAttendance()
		{
			var att = _attedance.GetAllAttendance().ToList();
			return att;
		}

		public List<Department_worker_Academic_performance> GetDepartment_worker_Academic_performance()
		{
			var depWo_acPerf = _department_worker_academic_performance.GetAllDepartment_worker_Academic_performance().ToList();
			return depWo_acPerf;
		}

		public List<Group_Subject> GetGroup_Subject()
		{
			var group_subj = _group_subject.GetAllGroup_Subject().ToList();
			return group_subj;
		}



		//Get таблиц

		//Страницы
		public async Task<IActionResult> ShowData(int selectedGroup, int selectedStudent)
		{
			if (selectedStudent > 0)//ТОЛЬКО УДАЛЕНИЕ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			{
				var StudentToRemove = _student.GetStudentbyID(selectedStudent).Result;
				Group GroupOfDeletedStudent = _group.GetGroupbyID(StudentToRemove.ID_Group).Result;
				await _student.DeleteStudent(StudentToRemove);
				var model = new GroupsStudents { Groups = GetGroups(), Students = GetStudents(), selectedGroup = GroupOfDeletedStudent };
				return View(model);
			}

			if (selectedGroup > 0)
			{
				var groupById = _group.GetGroupbyID(selectedGroup).Result;
				var model = new GroupsStudents { Groups = GetGroups(), Students = GetStudents(), selectedGroup = groupById };
				return View(model);
			}
			else
			{
				var model = new GroupsStudents { Groups = GetGroups(), Students = GetStudents(), selectedGroup = GetGroups().FirstOrDefault() };
				return View(model);
			}

		}

		public async Task<IActionResult> AddStudent()
		{
			var model2 = new DeleteStudentneedGroup {Groups = GetGroups()};
			return View(model2);
		}

		[HttpPost]
		public async Task<IActionResult> AddStudent(string surname, string name, string patronymic, int selectedGroup, DeleteStudentneedGroup del)
		{
			if (ModelState.IsValid)
			{
				var addStudent = new Student() { Surname = surname, Name = name, Patronymic = patronymic, ID_Group = selectedGroup };
				await _student.AddStudent(addStudent);
				return Content("Студент добавлен");
			}
			else
			{
				var model2 = new DeleteStudentneedGroup { Groups = GetGroups()};
				return View(model2);
			}
		}

		public IActionResult Index()
		{
			return View();
		}


		[HttpPost]
		public IActionResult Index(UserViewModel user)
		{
			if (ModelState.IsValid)
			{
				return Content(user.Login + " " + user.Password);
			}
			else return View(user);
		}




		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}


	}
}