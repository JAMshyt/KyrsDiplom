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
	public class StudentController : Controller
	{
		private readonly ILogger<StudentController> _logger;

		private readonly IStudent _student;
		private readonly IGroup _group;
		private readonly ISubject _subject;
		private readonly IGroup_Subject _group_subject;

		public StudentController(ILogger<StudentController> logger, IStudent student,
			IGroup group, ISubject subject,	IGroup_Subject group_subject
			)
		{
			_logger = logger;
			_student = student;
			_group = group;
			_subject = subject;
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
		#endregion

		public async Task<IActionResult> ShowStudents(int selectedGroup)
		{
			if (selectedGroup > 0)
			{
				var groupById = _group.GetGroupbyID(selectedGroup);
				var model = new GroupsStudents { Groups = GetGroups(), Students = GetStudents(), selectedGroup = groupById };
				return View(model);
			}
			else
			{
				var model = new GroupsStudents { Groups = GetGroups(), Students = GetStudents(), selectedGroup = GetGroups().FirstOrDefault() };
				return View(model);
			}
		}



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

		public async Task<IActionResult> AddStudent()
		{
			var model2 = new AddStudentViewModel { Groups = GetGroups(), ID_Group = GetGroups().FirstOrDefault().ID_Group, studentAdded = false };
			return View(model2);
		}

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
