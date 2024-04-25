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
		private readonly IDepartment_worker _department_worker;
		private readonly ICurator _curator;
		private readonly ILogins _logins;

		public AccountController(ILogger<AccountController> logger, IStudent student,
			IDepartment_worker department_worker, ICurator curator, IGroup group,
			ILogins logins
			)
		{
			_logger = logger;
			_student = student;
			_department_worker = department_worker;
			_curator = curator;
			_group = group;
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

		public List<Department_worker> GetDepartment_Workers()
		{
			var dep_works = _department_worker.GetAllDepartment_worker().ToList();
			return dep_works;
		}

		public List<Curator> GetCurators()
		{
			var curs = _curator.GetAllCurator().ToList();
			return curs;
		}

		public List<Logins> GetLogins()
		{
			var logs = _logins.GetAllLogins().ToList();
			return logs;
		}
		#endregion

		public ViewResult AccountInfo()
		{
			var model = new AccountViewModel();

			switch (User.FindFirst(ClaimTypes.Role)?.Value)
			{
				case "Student":
					var student = GetStudents().FirstOrDefault(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));
					var group = GetGroups().FirstOrDefault(q => q.ID_Group == student.ID_Group);

					byte[] photo = GetStudents().Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value)).Select(q => q.Photo).FirstOrDefault();
					string? photoBase64 = photo != null ? Convert.ToBase64String(photo): null ;

					model.Name = student.Name;
					model.Surname = student.Surname;
					model.Patronymic = student.Patronymic;
					model.Group = group;
					model.Photo = photoBase64;
					model.Graduating_department = group.Graduating_department;
					return View(model);
				case "Adm":
					var dep_work = GetDepartment_Workers().FirstOrDefault(q => q.ID_Department_worker == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));

					byte[] photoAdm = GetDepartment_Workers().Where(q => q.ID_Department_worker == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value)).Select(q => q.Photo).FirstOrDefault();
					string? photoBase64Adm = photoAdm != null ? Convert.ToBase64String(photoAdm) : null;

					model.Name = dep_work.Name;
					model.Surname = dep_work.Surname;
					model.Patronymic = dep_work.Patronymic;
					model.Photo = photoBase64Adm;
					model.Institute_title = dep_work.Institute_title;
					model.Job_title = dep_work.Job_title;
					return View(model);
				case "Curator":
					var cur = GetCurators().FirstOrDefault(q => q.ID_Curator == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));

					byte[] photoCur = GetCurators().Where(q => q.ID_Curator == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value)).Select(q => q.Photo).FirstOrDefault();
					string? photoBase64Cur = photoCur != null ? Convert.ToBase64String(photoCur) : null;

					model.Name = cur.Name;
					model.Surname = cur.Surname;
					model.Patronymic = cur.Patronymic;
					model.Photo = photoBase64Cur;
					return View(model);
			}

			return View(model);
		}


		[HttpGet] 
		[Route("/Account/AccountInfoStudent{idLogin:int}")] 
		public ViewResult AccountInfoStudent(int idLogin)
		{
			var person = GetStudents().FirstOrDefault(q => q.ID_Login == idLogin);
			var login = GetLogins().FirstOrDefault(q => q.ID_Login == person.ID_Login);
			byte[] photoCur = person.Photo;
			string? photoBase64 = photoCur != null ? Convert.ToBase64String(photoCur) : null;
			var model = new AccountViewModel()
			{
				Surname = person.Surname,
				Name = person.Name,
				Patronymic = person.Patronymic,
				Group = GetGroups().FirstOrDefault(q => q.ID_Group == person.ID_Group),
				Photo = photoBase64,
				Email = login.Email,
				Phone = login.Phone,
				AdminWatching = true,
			};
			return View("AccountInfo", model);
		}

	}
}
