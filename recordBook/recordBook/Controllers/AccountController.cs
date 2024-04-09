using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.Models.ViewModels;
using recordBook.RInterface;

namespace recordBook.Controllers
{
	public class AccountController : Controller
	{
		private readonly ILogger<AccountController> _logger;

		private readonly IStudent _student;
		private readonly IGroup _group;
		private readonly ISubject _subject;
		private readonly IKind_of_work _kind_of_work;
		private readonly IGroup_Subject _group_subject;
		private readonly IAcademic_performance _academic_performance;
		private readonly IDepartment_worker _department_worker;

		public AccountController(ILogger<AccountController> logger, IStudent student,
			IGroup group, ISubject subject, IKind_of_work kind_wf_work,
			IAcademic_performance academic_performance, IDepartment_worker department_worker,
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
		public List<Department_worker> GetDepartment_Workers()
		{
			var dep_works = _department_worker.GetAllDepartment_worker().ToList();
			return dep_works;
		}

		#endregion

		public ViewResult AccountInfo()
		{
			var model = new AccountViewModel();
			if (User.FindFirst(ClaimTypes.Role)?.Value == "Student")
			{
				var student = GetStudents().FirstOrDefault(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));

				byte[] photo = GetStudents().Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value)).Select(q => q.Photo).FirstOrDefault();
				string photoBase64 = Convert.ToBase64String(photo);

				model.Name = student.Name;
				model.Surname = student.Surname;
				model.Patronymic = student.Patronymic;
				model.Group = GetGroups().FirstOrDefault(q => q.ID_Group == student.ID_Group);
				model.Photo = photoBase64;
			}
			else
			{
				var dep_work = GetDepartment_Workers().FirstOrDefault(q => q.ID_Department_worker == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));

				byte[] photo = GetDepartment_Workers().Where(q => q.ID_Department_worker == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value)).Select(q => q.Photo).FirstOrDefault();
				string photoBase64 = Convert.ToBase64String(photo);

				model.Name = dep_work.Name;
				model.Surname = dep_work.Surname;
				model.Patronymic = dep_work.Patronymic;
				model.Photo = photoBase64;

			}
			return View(model);
		}


	}
}
