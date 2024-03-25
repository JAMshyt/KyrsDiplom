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
	public class ExamsController : Controller
	{

		private readonly ILogger<ExamsController> _logger;

		private readonly IStudent _student;
		private readonly IGroup _group;
		private readonly ISubject _subject;
		private readonly IKind_of_work _kind_of_work;
		private readonly IDepartment_worker _department_worker;
		private readonly IGroup_Subject _group_subject;
		private readonly IAcademic_performance _academic_performance;

		public ExamsController(ILogger<ExamsController> logger, IStudent student,
			IGroup group, ISubject subject, IKind_of_work kind_wf_work,
			IDepartment_worker department_worker, IAcademic_performance academic_performance,
			IGroup_Subject group_subject
			)
		{
			_logger = logger;
			_student = student;
			_group = group;
			_subject = subject;
			_kind_of_work = kind_wf_work;
			_department_worker = department_worker;
			_group_subject = group_subject;
			_academic_performance = academic_performance;
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

		public List<Group_Subject> GetGroup_Subject()
		{
			var group_subj = _group_subject.GetAllGroup_Subject().ToList();
			return group_subj;
		}

		public List<Academic_performance> GetAcademic_performance()
		{
			var acad_perf = _academic_performance.GetAllAcademic_performance().ToList();
			return acad_perf;
		}

		public List<Kind_of_work> GetKind_of_works()
		{
			var kind_of_work = _kind_of_work.GetAllKind_of_work().ToList();
			return kind_of_work;
		}

		#endregion

		public async Task<IActionResult> ExamsMarks(int selectedGroup, int selectedSubject)
		{

			if (selectedGroup > 0 & selectedSubject > 0)
			{
				var groupById = _group.GetGroupbyID(selectedGroup);
				var subjectsOfSelectedGroup = _group_subject.GetGroup_SubjectbyGroupID(selectedGroup).Select(z => z.ID_Subject);
				if (!subjectsOfSelectedGroup.Contains(selectedSubject))
				{
					selectedSubject = subjectsOfSelectedGroup.FirstOrDefault();
				}
				var subjectById = _subject.GetSubjectbyID(selectedSubject);
				var model = new Exams { Groups = GetGroups(), Students = GetStudents(), Group_Subjects = GetGroup_Subject(),
					Subjects = GetSubjects(), Academic_Performances = GetAcademic_performance(),
					selectedGroup = groupById, selectedSubject = subjectById,
					Kind_of_works = GetKind_of_works(),
				};
				return View(model);
			}
			else
			{
				var model = new Exams { Groups = GetGroups(), Students = GetStudents(), Group_Subjects = GetGroup_Subject(),
					Subjects = GetSubjects(), Academic_Performances = GetAcademic_performance(),
					selectedGroup = GetGroups().FirstOrDefault(), selectedSubject = GetSubjects().FirstOrDefault(),
					Kind_of_works = GetKind_of_works(),
				};
				return View(model);
			}

		}

		[HttpGet]
		[Route("Exams/ChangeGrades/")]
		public async Task<IActionResult> ChangeGrades()
		{
			return Json("работает");
		}

	}
}
