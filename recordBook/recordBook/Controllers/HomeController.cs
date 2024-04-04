using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
		private readonly ILogins _logins;

		public HomeController(ILogger<HomeController> logger, IStudent student,
			IGroup group, ISubject subject, IKind_of_work kind_wf_work,
			IDepartment_worker department_worker, IAcademic_performance academic_performance,
			IAttendance attendance, IDepartment_worker_Academic_performance department_worker_academic_performance,
			IGroup_Subject group_subject, ILogins logins
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

		public List<Logins> GetLogins()
		{
			var logins = _logins.GetAllLogins().ToList();
			return logins;
		}

		#endregion

		#region страницы

		public ViewResult Authorization()
		{
			AuthorizationViewModel user = new AuthorizationViewModel() { ErrorText = false };
			return View(user);
		}


		[HttpPost]
		public async Task<ActionResult> Authorization(AuthorizationViewModel user)
		{
			if (ModelState.IsValid)
			{
				Logins? login = GetLogins().FirstOrDefault(q => q.Login == user.Login && q.Password == user.Password);
				if (login is null)
				{
					user.ErrorText = true;
					return View(user);
				}
				else
				{
					user.ErrorText = false;
					try
					{
						Student? student = GetStudents().FirstOrDefault(q => q.ID_Login == login.ID_Login);

						var claims = new List<Claim> {
						new Claim(ClaimTypes.NameIdentifier, login.Login),
						new Claim(ClaimTypes.Email, login.Email),
						new Claim(ClaimTypes.Name, student.Name),
						new Claim(ClaimTypes.Surname, student.Surname),
						new Claim(ClaimTypes.GivenName, student.Patronymic),
						new Claim(ClaimTypes.Role, "Student"),
						new Claim(ClaimTypes.GroupSid, Convert.ToString(student.ID_Group)),
						new Claim(ClaimTypes.SerialNumber, Convert.ToString(student.ID_Student))
						};
						ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
						await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
						return RedirectToAction("ExamsMarks", "Exams");
					}
					catch
					{
						Department_worker? teacher = GetDepartment_worker().FirstOrDefault(q => q.ID_Login == login.ID_Login);

						var claims = new List<Claim> {
						new Claim(ClaimTypes.NameIdentifier, login.Login),
						new Claim(ClaimTypes.Email, login.Email),
						new Claim(ClaimTypes.Name, teacher.Name),
						new Claim(ClaimTypes.Surname, teacher.Surname),
						new Claim(ClaimTypes.GivenName, teacher.Patronymic),
						new Claim(ClaimTypes.Role, "Teacher"),
						};
						ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
						await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
						return RedirectToAction("ExamsMarks", "Exams");
					}
				}
			}
			else return View(user);
		}

		public ViewResult AttendanceOfStudents(int selectedGroup, int selectedSubject) /*async Task<IActionResult>*/
		{
			ViewData["User"] = User.FindFirst(ClaimTypes.Surname)?.Value + " " + User.FindFirst(ClaimTypes.Name)?.Value;
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

		#endregion

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}


	}
}