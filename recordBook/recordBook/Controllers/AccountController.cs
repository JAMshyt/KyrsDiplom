﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Numerics;
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
using static recordBook.Controllers.ExamsController;
using static recordBook.Controllers.StudentController;

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
		private readonly ILoginsStudent _loginsStudent;

		public AccountController(ILogger<AccountController> logger, IStudent student,
			IDepartment_worker department_worker, ICurator curator, IGroup group,
			ILogins logins, ILoginsStudent loginsStudent
			)
		{
			_logger = logger;
			_student = student;
			_department_worker = department_worker;
			_curator = curator;
			_group = group;
			_logins = logins;
			_loginsStudent = loginsStudent;
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

		public List<LoginsStudent> GetLoginsStudents()
		{
			var logs = _loginsStudent.GetAllLoginsStudent().ToList();
			return logs;
		}
		#endregion

		public ViewResult AccountInfo()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return View("Error");
			}
			else
			{
				var model = new AccountViewModel();

				switch (User.FindFirst(ClaimTypes.Role)?.Value)
				{
					case "Student":
						Student student = GetStudents().FirstOrDefault(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));
						Group group = GetGroups().FirstOrDefault(q => q.ID_Group == student.ID_Group);

						byte[] photo = GetStudents().Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value)).Select(q => q.Photo).FirstOrDefault();
						string? photoBase64 = photo != null ? Convert.ToBase64String(photo) : null;

						model.ID = student.ID_Student;
						model.Name = student.Name;
						model.Surname = student.Surname;
						model.Patronymic = student.Patronymic;
						model.Group = group;
						model.Photo = photoBase64;
						model.Graduating_department = group.Graduating_department;
						model.Financing_source = group.Financing_source;
						model.Groups = GetGroups();
						model.NumberOfBook = student.NumberOfBook;
						return View(model);
					case "Adm":
						var dep_work = GetDepartment_Workers().FirstOrDefault(q => q.ID_Department_worker == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));

						byte[] photoAdm = GetDepartment_Workers().Where(q => q.ID_Department_worker == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value)).Select(q => q.Photo).FirstOrDefault();
						string? photoBase64Adm = photoAdm != null ? Convert.ToBase64String(photoAdm) : null;

						model.ID = dep_work.ID_Department_worker;
						model.Name = dep_work.Name;
						model.Surname = dep_work.Surname;
						model.Patronymic = dep_work.Patronymic;
						model.Photo = photoBase64Adm;
						model.Institute_title = dep_work.Institute_title;
						model.Job_title = dep_work.Job_title;
						model.Groups = GetGroups();
						return View(model);
					case "Curator":
						var cur = GetCurators().FirstOrDefault(q => q.ID_Curator == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));

						byte[] photoCur = GetCurators().Where(q => q.ID_Curator == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value)).Select(q => q.Photo).FirstOrDefault();
						string? photoBase64Cur = photoCur != null ? Convert.ToBase64String(photoCur) : null;

						model.ID = cur.ID_Curator;
						model.Name = cur.Name;
						model.Surname = cur.Surname;
						model.Patronymic = cur.Patronymic;
						model.Photo = photoBase64Cur;
						model.Groups = GetGroups();
						return View(model);
				}

				return View(model);
			}
		}

		/// <summary>
		/// Админ просматривает профиля студентов
		/// </summary>
		/// <param name="idLogin"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("/Account/AccountInfoStudent{idLogin:int}")]
		public ViewResult AccountInfoStudent(int idLogin)
		{
			if (User.IsInRole("Adm"))
			{
				var person = GetStudents().FirstOrDefault(q => q.NumberOfBook == idLogin);
				var login = GetLoginsStudents().FirstOrDefault(q => q.Number_RecordBook == person.NumberOfBook);
				byte[] photoCur = person.Photo;
				string? photoBase64 = photoCur != null ? Convert.ToBase64String(photoCur) : null;
				var group = GetGroups().FirstOrDefault(q => q.ID_Group == person.ID_Group);

				var model = new AccountViewModel()
				{
					ID = person.ID_Student,
					Surname = person.Surname,
					Name = person.Name,
					Patronymic = person.Patronymic,
					Group = GetGroups().FirstOrDefault(q => q.ID_Group == person.ID_Group),
					Photo = photoBase64,
					Email = login.Email,
					Phone = login.Phone,
					AdminWatching = true,
					Graduating_department = group.Graduating_department,
					Financing_source = group.Financing_source,
					Groups = GetGroups(),
					NumberOfBook = person.NumberOfBook,
				};
				return View("AccountInfo", model);
			}
			else
			{
				return View("Error");
			}
		}

		#region классы для JSON
		public class IdAndFIO
		{
			public int Id { get; set; }
			public string Info { get; set; }
			public string Change { get; set; }
		}
		#endregion

		#region Изменение информации о пользователях
		/// <summary>
		/// Изменяет информацию о студенте
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		[HttpPost]
		[Route("/Account/InfoIsChanged/")]
		public async Task<IActionResult> InfoIsChanged([FromForm] IdAndFIO request)
		{
			if (User.IsInRole("Adm"))
			{
				Student oldStudent = GetStudents().FirstOrDefault(z => z.ID_Student == request.Id);

				switch (request.Change)
				{
					case "Name":
						oldStudent.Name = request.Info;
						await _student.UpdateStudent(oldStudent);
						break;

					case "Sur":
						oldStudent.Surname = request.Info;
						await _student.UpdateStudent(oldStudent);
						break;

					case "Patr":
						oldStudent.Patronymic = request.Info;
						await _student.UpdateStudent(oldStudent);
						break;

					case "Group":
						oldStudent.ID_Group = Convert.ToInt32(request.Info);
						await _student.UpdateStudent(oldStudent);
						break;

					case "Phone":
						LoginsStudent login = GetLoginsStudents().FirstOrDefault(x => x.Number_RecordBook == oldStudent.NumberOfBook);
						login.Phone = Convert.ToDecimal(request.Info);
						await _loginsStudent.UpdateLoginStudent(login);
						break;

					//case "Book":
					//	LoginsStudent login2 = GetLoginsStudents().FirstOrDefault(x => x.Number_RecordBook == oldStudent.NumberOfBook);
					//	login2.Number_RecordBook = Convert.ToInt32(request.Info);
					//	await _loginsStudent.UpdateLoginStudent(login2);
					//	break;

					case "Email":
						LoginsStudent login3 = GetLoginsStudents().FirstOrDefault(x => x.Number_RecordBook == oldStudent.NumberOfBook);
						login3.Email = request.Info;
						await _loginsStudent.UpdateLoginStudent(login3);
						break;

					case "Photo":
						var file = Request.Form.Files.GetFile("file");
						if (file != null)
						{
							using (var memoryStream = new MemoryStream())
							{
								await file.CopyToAsync(memoryStream);
								oldStudent.Photo = memoryStream.ToArray();
							}
							await _student.UpdateStudent(oldStudent);
						}
						break;
				}
				return Json("");
			}
			else
			{
				return Redirect("/Error");
			}
		}
		#endregion
	}
}
