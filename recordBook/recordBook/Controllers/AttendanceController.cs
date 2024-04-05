using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using recordBook.Models;
using recordBook.Models.ViewModels;
using recordBook.RInterface;

namespace recordBook.Controllers
{
	public class AttendanceController : Controller
	{
		private readonly ILogger<AttendanceController> _logger;

		private readonly IStudent _student;
		private readonly IGroup _group;
		private readonly ISubject _subject;
		private readonly IAttendance _attedance;
		private readonly IGroup_Subject _group_subject;


		public AttendanceController(ILogger<AttendanceController> logger, IStudent student,
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
			_attedance = attendance;
			_group_subject = group_subject;
		}

		#region таблицы
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

		public List<Attendance> GetAttendance()
		{
			var att = _attedance.GetAllAttendance().ToList();
			return att;
		}

		public List<Group_Subject> GetGroup_Subject()
		{
			var group_subj = _group_subject.GetAllGroup_Subject().ToList();
			return group_subj;
		}

		#endregion

		public ViewResult AttendanceOfStudents(int selectedGroup, int selectedSubject)
		{
			ViewData["User"] = User.FindFirst(ClaimTypes.Surname)?.Value + " " + User.FindFirst(ClaimTypes.Name)?.Value;

			var model = new AttendanceViewModel
			{
				Groups = GetGroups(),
				Students = GetStudents(),
				Group_Subjects = GetGroup_Subject(),
				Subjects = GetSubjects(),
				Attendances = GetAttendance(),
				selectedGroup = GetGroups().FirstOrDefault(),
				selectedSubject = GetSubjects().FirstOrDefault()
			};

			if (User.IsInRole("Teacher"))
			{
				if (selectedGroup > 0 & selectedSubject > 0)
				{
					model.selectedGroup = _group.GetGroupbyID(selectedGroup);
					model.selectedSubject = _subject.GetSubjectbyID(selectedSubject);

				}
			}
			else
			{
				model.Groups = GetGroups().Where(q=>q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
				model.Students = GetStudents().Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));
				model.Group_Subjects = GetGroup_Subject().Where(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
			}
			return View(model);
		}
	}
}
