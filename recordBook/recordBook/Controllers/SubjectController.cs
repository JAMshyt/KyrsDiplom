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
	public class SubjectController : Controller
	{
		private readonly ILogger<SubjectController> _logger;

		private readonly IGroup _group;
		private readonly ISubject _subject;
		private readonly IGroup_Subject _group_subject;

		public SubjectController(ILogger<SubjectController> logger, IGroup group, ISubject subject, IGroup_Subject group_subject
			)
		{
			_logger = logger;
			_group = group;
			_subject = subject;
			_group_subject = group_subject;
		}


		#region Get таблиц
		public List<Subject> GetSubjects()
		{
			var subjects = _subject.GetAllSubject().ToList();
			return subjects;
		}

		public List<Group> GetGroups()
		{
			var group = _group.GetAllGroup().ToList();
			return group;
		}

		public List<Group_Subject> GetGroupsSubject()
		{
			var groupSubj = _group_subject.GetAllGroup_Subject().ToList();
			return groupSubj;
		}

		#endregion

		/// <summary>
		/// открывает страницу добавления предмета
		/// </summary>
		/// <returns></returns>
		public ViewResult AddSubject()
		{
			if (User.IsInRole("Adm"))
			{
				ViewData["User"] = User.FindFirst(ClaimTypes.Surname)?.Value + " " + User.FindFirst(ClaimTypes.Name)?.Value;
				var model2 = new AddSubjectViewModel { Groups = GetGroups(), subjectAdded = false };
				return View(model2);
			}
			else
			{
				return View("Error");
			}
		}

		/// <summary>
		/// проверяет заполненость нужных полей, добавляет предмет
		/// </summary>
		/// <param name="AddSubj">JSON файл с моделью студента</param>
		/// <returns>JSON файл с странице и информацией о студенте и результатом о его добавлении</returns>
		[HttpPost]
		[Consumes("application/json")]
		public async Task<IActionResult> AddSubject([FromBody] AddSubjectViewModel AddSubj)
		{
			if (User.IsInRole("Adm"))
			{
				if (ModelState.IsValid)
				{
					var addSubject = new Subject() { Name_subject = AddSubj.NameSubject };
					await _subject.AddSubject(addSubject);
					foreach (var r in AddSubj.selectedGroups)
					{
						var addGroupSubject = new Group_Subject() { ID_Subject = GetSubjects().LastOrDefault().ID_Subject, ID_Group = r };
						await _group_subject.AddGroup_Subject(addGroupSubject);
					}
					var model = new
					{
						NameSubject = addSubject.Name_subject,
						Groups = GetGroups(),
						subjectAdded = true,
						AddSubj.selectedGroups,
					};
					return Json(model);
				}
				else
				{
					var model2 = new AddSubjectViewModel { Groups = GetGroups(), subjectAdded = false, selectedGroups = AddSubj.selectedGroups, NameSubject = AddSubj.NameSubject };
					return Json(model2); ;
				}
			}
			else
			{
				return RedirectToAction("Error", "Home");
			}
		}

		/// <summary>
		/// ищет нужную группу, чтобы добавить её в таблицу выбранных групп
		/// </summary>
		/// <param name="Id">id группы</param>
		/// <returns>JSON файл с информацией о группе</returns>
		[HttpGet]
		[Route("Subject/SelectGroup/{Id:int}")]
		public JsonResult SelectGroup(int Id)
		{
			if (User.IsInRole("Adm"))
			{
				return Json(_group.GetGroupbyID(Id));
			}
			else
			{
				return Json("no");
			}
		}


		/// <summary>
		/// Меняем предметы для изучения группе
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Subject/ChangeGroupsSubject{Id:int}")]
		public ViewResult ChangeGroupsSubject(int Id)
		{
			if (User.IsInRole("Adm"))
			{
				GroupSubjectViewModel model = new GroupSubjectViewModel()
				{
					group = GetGroups().FirstOrDefault(q => q.ID_Group == Id),
					subjects = GetSubjects(),
					group_Subject = GetGroupsSubject().Where(q => q.ID_Group == Id),
				};
				return View(model);
			}
			else
			{
				return View("Error");
			}
		}


		public class IdAndGroupId
		{
			public int id { get; set; }
			public int group { get; set; }
		}

		[HttpPost]
		[Route("Subject/AddNewSubjectToLearn/")]
		public async Task<IActionResult> AddNewSubjectToLearn([FromBody] IdAndGroupId req)
		{
			if (User.IsInRole("Adm"))
			{
				Group_Subject newSubjGroup = new Group_Subject()
				{
					ID_Group = req.group,
					ID_Subject = req.id,
				};
				await _group_subject.AddGroup_Subject(newSubjGroup);
				return Json(newSubjGroup);
			}
			else
			{
				return Json("no");
			}
		}

		[HttpPost]
		[Route("Subject/RemoveSubjectToLearn/")]
		public async Task<IActionResult> RemoveSubjectToLearn([FromBody] IdAndGroupId req)
		{
			if (User.IsInRole("Adm"))
			{
				Group_Subject newSubjGroup = GetGroupsSubject().FirstOrDefault(q => q.ID_Subject == req.id & q.ID_Group == req.group);

				await _group_subject.DeleteGroup_Subject(newSubjGroup);
				return Json(newSubjGroup);
			}
			else
			{
				return Json("no");
			}
		}

	}
}
