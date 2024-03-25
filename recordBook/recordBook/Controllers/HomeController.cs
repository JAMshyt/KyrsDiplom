using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
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

		#endregion

		#region страницы

		//public async Task<IActionResult> ShowData(int selectedGroup)
		//{
		//	if (selectedGroup > 0)
		//	{
		//		var groupById = _group.GetGroupbyID(selectedGroup);
		//		var model = new GroupsStudents { Groups = GetGroups(), Students = GetStudents(), selectedGroup = groupById };
		//		return View(model);
		//	}
		//	else
		//	{
		//		var model = new GroupsStudents { Groups = GetGroups(), Students = GetStudents(), selectedGroup = GetGroups().FirstOrDefault() };
		//		return View(model);
		//	}
		//}

		//public async Task<IActionResult> ExamsMarks(int selectedGroup, int selectedSubject)
		//{

		//	if (selectedGroup > 0 & selectedSubject > 0)
		//	{
		//		var groupById = _group.GetGroupbyID(selectedGroup);
		//		var subjectsOfSelectedGroup = _group_subject.GetGroup_SubjectbyGroupID(selectedGroup).Select(z => z.ID_Subject);
		//		if (!subjectsOfSelectedGroup.Contains(selectedSubject))
		//		{
		//			selectedSubject = subjectsOfSelectedGroup.FirstOrDefault();
		//		}
		//		var subjectById = _subject.GetSubjectbyID(selectedSubject);
		//		var model = new Exams { Groups = GetGroups(), Students = GetStudents(), Group_Subjects = GetGroup_Subject(), Subjects = GetSubjects(), Academic_Performances = GetAcademic_performance(), selectedGroup = groupById, selectedSubject = subjectById };
		//		return View(model);
		//	}
		//	else
		//	{
		//		var model = new Exams { Groups = GetGroups(), Students = GetStudents(), Group_Subjects = GetGroup_Subject(), Subjects = GetSubjects(), Academic_Performances = GetAcademic_performance(), selectedGroup = GetGroups().FirstOrDefault(), selectedSubject = GetSubjects().FirstOrDefault() };
		//		return View(model);
		//	}

		//}


		public ViewResult AttendanceOfStudents(int selectedGroup, int selectedSubject) /*async Task<IActionResult>*/
		{

			if (selectedGroup > 0 & selectedSubject > 0)
			{
				var groupById = _group.GetGroupbyID(selectedGroup);
				var subjectById = _subject.GetSubjectbyID(selectedSubject);
				var model = new AttendanceViewModel { Groups = GetGroups(), Students = GetStudents(), Group_Subjects = GetGroup_Subject(), Subjects = GetSubjects(), Attendances = GetAttendance(), selectedGroup = groupById, selectedSubject = subjectById };
				return View(model);
			}
			else
			{
				var model = new AttendanceViewModel { Groups = GetGroups(), Students = GetStudents(), Group_Subjects = GetGroup_Subject(), Subjects = GetSubjects(), Attendances = GetAttendance(), selectedGroup = GetGroups().FirstOrDefault(), selectedSubject = GetSubjects().FirstOrDefault() };
				return View(model);
			}
		}

		//public async Task<IActionResult> AddStudent()
		//{
		//	var model2 = new AddStudentViewModel { Groups = GetGroups(), ID_Group = GetGroups().FirstOrDefault().ID_Group, studentAdded = false };
		//	return View(model2);
		//}

		//[HttpPost]
		//public async Task<IActionResult> AddStudent(AddStudentViewModel addStu)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var addStudent = new Student() { Surname = addStu.Surname, Name = addStu.Name, Patronymic = addStu.Patronymic, ID_Group = addStu.ID_Group };
		//		await _student.AddStudent(addStudent);
		//		var model2 = new AddStudentViewModel { Surname = addStu.Surname, Name = addStu.Name, Patronymic = addStu.Patronymic, ID_Group = addStu.ID_Group, Groups = GetGroups(), studentAdded = true };
		//		return View(model2);
		//	}
		//	else
		//	{
		//		var model2 = new AddStudentViewModel { Groups = GetGroups(), studentAdded = false };
		//		return View(model2);
		//	}
		//}

		//[HttpGet]
		//[Route("Home/DropStudent/{Id:int}")]
		//public async Task<IActionResult> DropStudent(int Id)
		//{
		//	var stu = _student.GetStudentbyID(Id);
		//	if (stu != null)
		//	{
		//		await _student.DeleteStudent(stu);
		//	}
		//	return RedirectToAction(nameof(ShowData));
		//}



		//public async Task<IActionResult> AddSubject()
		//{
		//	var model2 = new AddSubjectViewModel { Groups = GetGroups(), subjectAdded = false };
		//	return View(model2);
		//}


		//[HttpPost]
		//[Consumes("application/json")]
		//public async Task<IActionResult> AddSubject([FromBody] AddSubjectViewModel AddSubj)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var addSubject = new Subject() { Name_subject = AddSubj.NameSubject };
		//		await _subject.AddSubject(addSubject);
		//		foreach (var r in AddSubj.selectedGroups)
		//		{
		//			var addGroupSubject = new Group_Subject() { ID_Subject = GetSubjects().LastOrDefault().ID_Subject, ID_Group = r };
		//			await _group_subject.AddGroup_Subject(addGroupSubject);
		//		}
		//		var model = new
		//		{
		//			NameSubject = addSubject.Name_subject,
		//			Groups = GetGroups(),
		//			subjectAdded = true,
		//			selectedGroups = AddSubj.selectedGroups,
		//		};
		//		return Json(model);
		//	}
		//	else
		//	{
		//		var model2 = new AddSubjectViewModel { Groups = GetGroups(), subjectAdded = false, selectedGroups = AddSubj.selectedGroups, NameSubject = AddSubj.NameSubject };
		//		return Json(model2); ;
		//	}
		//}


		//[HttpGet]
		//[Route("Home/SelectGroup/{Id:int}")]
		//public async Task<IActionResult> SelectGroup(int Id)
		//{
		//	return Json(_group.GetGroupbyID(Id));
		//}


		#endregion



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