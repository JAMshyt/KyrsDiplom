using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
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
	public class StudentController : Controller
	{
		private readonly ILogger<StudentController> _logger;

		private readonly IStudent _student;
		private readonly IGroup _group;
		private readonly ISubject _subject;
		private readonly IGroup_Subject _group_subject;

		private readonly ILogins _user;

		public StudentController(ILogger<StudentController> logger, IStudent student,
			IGroup group, ISubject subject,	IGroup_Subject group_subject, ILogins user
			)
		{
			_logger = logger;
			_student = student;
			_group = group;
			_subject = subject;
			_group_subject = group_subject;
			_user = user;
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
		#endregion


		/// <summary>
		/// выводит всех студентов по группам
		/// </summary>
		/// <param name="selectedGroup">id выбранной группы</param>
		/// <returns>модел с всеми учениками выбранной группы</returns>
		public async Task<IActionResult> ShowStudents(int selectedGroup)
		{
			if (selectedGroup > 0)
			{
				var groupById = _group.GetGroupbyID(selectedGroup);
				var model = new GroupsStudentsViewModel {UserName = User.Identity.Name, Groups = GetGroups(), Students = GetStudents(), selectedGroup = groupById };
				return View(model);
			}
			else
			{
				var model = new GroupsStudentsViewModel { UserName = User.FindFirst(ClaimTypes.Name)?.Value, Groups = GetGroups(), Students = GetStudents(), selectedGroup = GetGroups().FirstOrDefault() };
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
			var model2 = new AddStudentViewModel { Groups = GetGroups(), ID_Group = GetGroups().FirstOrDefault().ID_Group, studentAdded = false };
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
			if (ModelState.IsValid)
			{
				var addStudent = new Student() { Surname = addStu.Surname, Name = addStu.Name, Patronymic = addStu.Patronymic, ID_Group = addStu.ID_Group };
				await _student.AddStudent(addStudent);
				var model2 = new AddStudentViewModel { Surname = addStu.Surname, Name = addStu.Name, Patronymic = addStu.Patronymic, ID_Group = addStu.ID_Group, Groups = GetGroups(), studentAdded = true };
				return View(model2);
			}
			else
			{
				var model2 = new AddStudentViewModel { Groups = GetGroups(), studentAdded = false };
				return View(model2);
			}
		}

	}
}
