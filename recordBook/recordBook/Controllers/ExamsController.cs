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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using recordBook.Migrations;
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
		//private readonly IDepartment_worker _department_worker;
		private readonly IGroup_Subject _group_subject;
		private readonly IAcademic_performance _academic_performance;
		private readonly IRatingControl _ratingControl;

		public ExamsController(ILogger<ExamsController> logger, IStudent student,
			IGroup group, ISubject subject, IKind_of_work kind_wf_work,
			/*IDepartment_worker department_worker,*/ IAcademic_performance academic_performance,
			IGroup_Subject group_subject, IRatingControl ratingControl
			)
		{
			_logger = logger;
			_student = student;
			_group = group;
			_subject = subject;
			_kind_of_work = kind_wf_work;
			//_department_worker = department_worker;
			_group_subject = group_subject;
			_academic_performance = academic_performance;
			_ratingControl = ratingControl;
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

		public List<RatingControl> GetRatingControls()
		{
			var rait = _ratingControl.GetAllRatingControl().ToList();
			return rait;
		}
		#endregion


		/// <summary>
		/// Выводит все оценки по всем экзамнам. Сортирует их по группам и предметам
		/// </summary>
		/// <param name="selectedGroup">id выбранной группы</param>
		/// <param name="selectedSubject">id выбранного предмета</param>
		/// <returns>модель с информацией об оценках всех студентов выбранной группы по выбранному предмету</returns>
		public async Task<IActionResult> ExamsMarks(int selectedGroup, int selectedSubject)
		{
			ViewData["User"] = User.FindFirst(ClaimTypes.Surname)?.Value + " " + User.FindFirst(ClaimTypes.Name)?.Value;

			var model = new ExamsViewModel
			{
				Groups = GetGroups(),
				Students = GetStudents(),
				Group_Subjects = GetGroup_Subject(),
				Subjects = GetSubjects(),
				Academic_Performances = GetAcademic_performance(),
				selectedGroup = GetGroups().FirstOrDefault(),
				selectedSubject = GetSubjects().FirstOrDefault(),
				Kind_of_works = GetKind_of_works(),
			};

			switch (User.FindFirst(ClaimTypes.Role)?.Value)
			{
				case "Adm":
					if (selectedGroup > 0 & selectedSubject > 0)
					{
						var groupById = _group.GetGroupbyID(selectedGroup);
						var subjectsOfSelectedGroup = _group_subject.GetGroup_SubjectbyGroupID(selectedGroup).Select(z => z.ID_Subject);
						if (!subjectsOfSelectedGroup.Contains(selectedSubject))
						{
							selectedSubject = subjectsOfSelectedGroup.FirstOrDefault();
						}
						var subjectById = _subject.GetSubjectbyID(selectedSubject);
						model.selectedGroup = groupById;
						model.selectedSubject = subjectById;
					}
					return View(model);

				case "Student":
					model.Groups = GetGroups().Where(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
					model.Students = GetStudents().Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));
					model.Academic_Performances = GetAcademic_performance().Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));
					model.selectedGroup = GetGroups().FirstOrDefault(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
					model.Group_Subjects = GetGroup_Subject().Where(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
					if (selectedSubject > 0)
					{
						var subjectById = _subject.GetSubjectbyID(selectedSubject);
						model.selectedSubject = subjectById;
					}
					return View(model);

				case "Curator":

					var groupClaims = User.FindAll(ClaimTypes.GroupSid).Select(claim => Convert.ToInt32(claim.Value)).ToList();
					var selectedGroups = GetGroups().Where(group => groupClaims.Contains(group.ID_Group));
					model.Groups = selectedGroups;
					model.Group_Subjects = GetGroup_Subject().Where(subject => selectedGroups.Any(group => group.ID_Group == subject.ID_Group));
					model.selectedGroup = selectedGroups.FirstOrDefault();

					if (selectedGroup > 0 & selectedSubject > 0)
					{
						model.selectedGroup = _group.GetGroupbyID(selectedGroup);
						model.selectedSubject = _subject.GetSubjectbyID(selectedSubject);
					}

					return View(model);
			}
			return View(model);
		}


		public async Task<IActionResult> Debt(int selectedGroup, int selectedSubject)
		{
			ViewData["User"] = User.FindFirst(ClaimTypes.Surname)?.Value + " " + User.FindFirst(ClaimTypes.Name)?.Value;

			var model = new ExamsViewModel
			{
				Groups = GetGroups(),
				Students = GetStudents(),
				Group_Subjects = GetGroup_Subject(),
				Subjects = GetSubjects(),
				Academic_Performances = GetAcademic_performance(),
				selectedGroup = GetGroups().FirstOrDefault(),
				selectedSubject = GetSubjects().FirstOrDefault(),
				Kind_of_works = GetKind_of_works(),
			};

			switch (User.FindFirst(ClaimTypes.Role)?.Value)
			{
				case "Adm":
					if (selectedGroup > 0 & selectedSubject > 0)
					{
						var groupById = _group.GetGroupbyID(selectedGroup);
						var subjectsOfSelectedGroup = _group_subject.GetGroup_SubjectbyGroupID(selectedGroup).Select(z => z.ID_Subject);
						if (!subjectsOfSelectedGroup.Contains(selectedSubject))
						{
							selectedSubject = subjectsOfSelectedGroup.FirstOrDefault();
						}
						var subjectById = _subject.GetSubjectbyID(selectedSubject);
						model.selectedGroup = groupById;
						model.selectedSubject = subjectById;
					}
					return View(model);

				case "Student":
					model.Groups = GetGroups().Where(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
					model.Students = GetStudents().Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));
					model.Academic_Performances = GetAcademic_performance()
						.Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));
					model.selectedGroup = GetGroups().FirstOrDefault(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
					model.Group_Subjects = GetGroup_Subject().Where(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));


					if (selectedSubject > 0)
					{
						var subjectById = _subject.GetSubjectbyID(selectedSubject);
						model.selectedSubject = subjectById;
					}
					return View(model);

				case "Curator":

					var groupClaims = User.FindAll(ClaimTypes.GroupSid).Select(claim => Convert.ToInt32(claim.Value)).ToList();
					var selectedGroups = GetGroups().Where(group => groupClaims.Contains(group.ID_Group));
					model.Groups = selectedGroups;
					model.Group_Subjects = GetGroup_Subject().Where(subject => selectedGroups.Any(group => group.ID_Group == subject.ID_Group));
					model.selectedGroup = selectedGroups.FirstOrDefault();

					if (selectedGroup > 0 & selectedSubject > 0)
					{
						model.selectedGroup = _group.GetGroupbyID(selectedGroup);
						model.selectedSubject = _subject.GetSubjectbyID(selectedSubject);
					}

					return View(model);
			}
			return View(model);
		}


		public async Task<IActionResult> Upcoming(int selectedGroup, int selectedSubject)
		{
			ViewData["User"] = User.FindFirst(ClaimTypes.Surname)?.Value + " " + User.FindFirst(ClaimTypes.Name)?.Value;

			var model = new ExamsViewModel
			{
				Groups = GetGroups(),
				Students = GetStudents(),
				Group_Subjects = GetGroup_Subject(),
				Subjects = GetSubjects(),
				Academic_Performances = GetAcademic_performance(),
				selectedGroup = GetGroups().FirstOrDefault(),
				selectedSubject = GetSubjects().FirstOrDefault(),
				Kind_of_works = GetKind_of_works(),
			};

			switch (User.FindFirst(ClaimTypes.Role)?.Value)
			{
				case "Adm":
					if (selectedGroup > 0 & selectedSubject > 0)
					{
						var groupById = _group.GetGroupbyID(selectedGroup);
						var subjectsOfSelectedGroup = _group_subject.GetGroup_SubjectbyGroupID(selectedGroup).Select(z => z.ID_Subject);
						if (!subjectsOfSelectedGroup.Contains(selectedSubject))
						{
							selectedSubject = subjectsOfSelectedGroup.FirstOrDefault();
						}
						var subjectById = _subject.GetSubjectbyID(selectedSubject);
						model.selectedGroup = groupById;
						model.selectedSubject = subjectById;
					}
					return View(model);


				case "Student":
					model.Groups = GetGroups().Where(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
					model.Students = GetStudents().Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));
					model.Academic_Performances = GetAcademic_performance()
						.Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));
					model.selectedGroup = GetGroups().FirstOrDefault(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
					model.Group_Subjects = GetGroup_Subject().Where(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));


					if (selectedSubject > 0)
					{
						var subjectById = _subject.GetSubjectbyID(selectedSubject);
						model.selectedSubject = subjectById;
					}
					return View(model);


				case "Curator":

					var groupClaims = User.FindAll(ClaimTypes.GroupSid).Select(claim => Convert.ToInt32(claim.Value)).ToList();
					var selectedGroups = GetGroups().Where(group => groupClaims.Contains(group.ID_Group));
					model.Groups = selectedGroups;
					model.Group_Subjects = GetGroup_Subject().Where(subject => selectedGroups.Any(group => group.ID_Group == subject.ID_Group));
					model.selectedGroup = selectedGroups.FirstOrDefault();

					if (selectedGroup > 0 & selectedSubject > 0)
					{
						model.selectedGroup = _group.GetGroupbyID(selectedGroup);
						model.selectedSubject = _subject.GetSubjectbyID(selectedSubject);
					}

					return View(model);
			}

			return View(model);
		}



		public async Task<IActionResult> Practices(int selectedGroup, int selectedSubject)
		{
			ViewData["User"] = User.FindFirst(ClaimTypes.Surname)?.Value + " " + User.FindFirst(ClaimTypes.Name)?.Value;

			var model = new ExamsViewModel
			{
				Groups = GetGroups(),
				Students = GetStudents(),
				Group_Subjects = GetGroup_Subject(),
				Subjects = GetSubjects(),
				Academic_Performances = GetAcademic_performance(),
				selectedGroup = GetGroups().FirstOrDefault(),
				selectedSubject = GetSubjects().FirstOrDefault(),
				Kind_of_works = GetKind_of_works(),
			};

			switch (User.FindFirst(ClaimTypes.Role)?.Value)
			{
				case "Adm":
					if (selectedGroup > 0 | selectedSubject > 0)
					{
						var groupById = _group.GetGroupbyID(selectedGroup);
						var subjectsOfSelectedGroup = _group_subject.GetGroup_SubjectbyGroupID(selectedGroup).Select(z => z.ID_Subject);
						if (!subjectsOfSelectedGroup.Contains(selectedSubject))
						{
							try
							{
								var subj = _subject.GetSubjectbyID(GetGroup_Subject()
									.FirstOrDefault(q => q.ID_Group == groupById.ID_Group
									&& GetAcademic_performance().Any(w => w.ID_Kind_of_work == 4 && w.ID_Subject == q.ID_Subject)).ID_Subject);
								model.selectedSubject = subj;
							}
							catch
							{

								selectedSubject = subjectsOfSelectedGroup.FirstOrDefault();
							}
						}
						else
						{
							var subjectById = _subject.GetSubjectbyID(selectedSubject);
							model.selectedSubject = subjectById;
						}
						model.selectedGroup = groupById;
					}
					return View(model);


				case "Student":
					model.Groups = GetGroups().Where(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
					model.Students = GetStudents().Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));
					model.Academic_Performances = GetAcademic_performance()
						.Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));
					model.selectedGroup = GetGroups().FirstOrDefault(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
					model.Group_Subjects = GetGroup_Subject().Where(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));


					if (selectedSubject > 0)
					{
						var subjectById = _subject.GetSubjectbyID(selectedSubject);
						model.selectedSubject = subjectById;
					}
					return View(model);


				case "Curator":

					var groupClaims = User.FindAll(ClaimTypes.GroupSid).Select(claim => Convert.ToInt32(claim.Value)).ToList();
					var selectedGroups = GetGroups().Where(group => groupClaims.Contains(group.ID_Group));
					model.Groups = selectedGroups;
					model.Group_Subjects = GetGroup_Subject().Where(subject => selectedGroups.Any(group => group.ID_Group == subject.ID_Group));

					if (selectedSubject > 0)
					{
						model.selectedSubject = _subject.GetSubjectbyID(selectedSubject);
					}
					else
					{
						var subjectsOfSelectedGroup = _group_subject.GetGroup_SubjectbyGroupID(selectedGroups.FirstOrDefault().ID_Group).Select(z => z.ID_Subject);

						var subj = _subject?.GetSubjectbyID(GetGroup_Subject()
							.FirstOrDefault(q => q.ID_Group == selectedGroups.FirstOrDefault()?.ID_Group
							&& GetAcademic_performance().Any(w => w.ID_Kind_of_work == 4 && w?.ID_Subject == q.ID_Subject))?.ID_Subject);

						model.selectedSubject = subj == null ? _subject.GetSubjectbyID(subjectsOfSelectedGroup.FirstOrDefault()) : subj;
					}
					model.selectedGroup = selectedGroup <1 ? selectedGroups.FirstOrDefault() : _group.GetGroupbyID(selectedGroup);

					return View(model);
			}
			return View(model);
		}




		public async Task<IActionResult> RatingList(int selectedGroup, int selectedSubject, int SelectedSemester)
		{
			ViewData["User"] = User.FindFirst(ClaimTypes.Surname)?.Value + " " + User.FindFirst(ClaimTypes.Name)?.Value;

			var maxSemester = GetRatingControls()
				.Where(q => q.ID_Student == GetStudents().FirstOrDefault(q => q.ID_Group == GetGroups().FirstOrDefault().ID_Group).ID_Student)
				.Max(q => q.Semester);

			var model = new ExamsViewModel
			{
				Groups = GetGroups(),
				Students = GetStudents(),
				Group_Subjects = GetGroup_Subject(),
				Subjects = GetSubjects(),
				selectedGroup = GetGroups().FirstOrDefault(),
				selectedSubject = GetSubjects().FirstOrDefault(),
				RatingControls = GetRatingControls(),
				selectedSemester = maxSemester,
			};

			switch (User.FindFirst(ClaimTypes.Role)?.Value)
			{
				case "Adm":
					if (selectedGroup > 0 & selectedSubject > 0 & SelectedSemester > 0)
					{
						var groupById = _group.GetGroupbyID(selectedGroup);
						var subjectsOfSelectedGroup = _group_subject.GetGroup_SubjectbyGroupID(selectedGroup).Select(z => z.ID_Subject);
						if (!subjectsOfSelectedGroup.Contains(selectedSubject))
						{
							selectedSubject = subjectsOfSelectedGroup.FirstOrDefault();
						}
						var subjectById = _subject.GetSubjectbyID(selectedSubject);
						model.selectedGroup = groupById;
						model.selectedSubject = subjectById;
						model.selectedSemester = SelectedSemester;

					}
					return View(model);


				case "Student":
					model.Groups = GetGroups().Where(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
					model.Students = GetStudents().Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value));
					model.selectedGroup = GetGroups().FirstOrDefault(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
					model.Group_Subjects = GetGroup_Subject().Where(q => q.ID_Group == Convert.ToInt32(User.FindFirst(ClaimTypes.GroupSid)?.Value));
					var maxSem = GetRatingControls()
	.Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value))
	.Max(q => q.Semester);
					model.selectedSemester = SelectedSemester < 1 ? maxSem : SelectedSemester;
					model.RatingControls = GetRatingControls().Where(q => q.ID_Student == Convert.ToInt32(User.FindFirst(ClaimTypes.SerialNumber)?.Value) );


					if (selectedSubject > 0)
					{
						var subjectById = _subject.GetSubjectbyID(selectedSubject);
						model.selectedSubject = subjectById;
					}

					return View(model);


				case "Curator":

					var groupClaims = User.FindAll(ClaimTypes.GroupSid).Select(claim => Convert.ToInt32(claim.Value)).ToList();
					var selectedGroups = GetGroups().Where(group => groupClaims.Contains(group.ID_Group));
					model.Groups = selectedGroups;
					model.Group_Subjects = GetGroup_Subject().Where(subject => selectedGroups.Any(group => group.ID_Group == subject.ID_Group));
					model.selectedGroup = selectedGroups.FirstOrDefault();

					if (selectedGroup > 0 & selectedSubject > 0)
					{
						model.selectedGroup = _group.GetGroupbyID(selectedGroup);
						model.selectedSubject = _subject.GetSubjectbyID(selectedSubject);
					}

					var sem = GetRatingControls()
						.Where(q => q.ID_Student == GetStudents().FirstOrDefault(q => q.ID_Group == selectedGroups.FirstOrDefault()?.ID_Group)?.ID_Student)
						.Max(q => q.Semester);

					model.selectedSemester = SelectedSemester < 1 ? sem : SelectedSemester;

					return View(model);
			}

			return View(model);
		}

		[HttpGet]
		[Route("/Exams/DebtsOfStudent{idStudent:int}")]
		public ViewResult DebtsOfStudent(int idStudent)
		{
			var model = new ExamsViewModel()
			{
				Students = GetStudents().Where(q => q.ID_Student == idStudent),
				Academic_Performances = GetAcademic_performance().Where(q => q.ID_Student == idStudent),
				Kind_of_works = GetKind_of_works(),
				Subjects = GetSubjects()
			};
			return View(model);
		}

		#region классы для JSON запросов
		public class IdAndGrade
		{
			public int Id { get; set; }
			public string newGrade { get; set; }

		}
		public class IdAndDate
		{
			public int Id { get; set; }
			public DateTime newDate { get; set; }
		}

		public class IdAndPoints
		{
			public int Id { get; set; }
			public int newPoints { get; set; }

		}

		public class newRat
		{
			public int IdStudent { get; set; }
			public int IdSubject { get; set; }
			public int Semester { get; set; }
			public int NumberRating { get; set; }
			public int newPoints { get; set; }

		}

		#endregion


		/// <summary>
		/// Изменяет оценки за экзамены
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("/Exams/ChangeGrades/")]
		[Consumes("application/json")]
		public async Task<IActionResult> ChangeGrades([FromBody] IdAndGrade request)
		{

			Academic_performance oldAcademPerf = GetAcademic_performance().FirstOrDefault(z => z.ID_Academic_performance == request.Id);
			oldAcademPerf.Grade = request.newGrade;
			await _academic_performance.UpdateAcademic_performance(oldAcademPerf);
			var str = request.Id + " " + request.newGrade;
			return Json(str);
		}

		/// <summary>
		/// изменяет дату сдачи для студента
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("/Exams/ChangeDates/")]
		[Consumes("application/json")]
		public async Task<IActionResult> ChangeDates([FromBody] IdAndDate request)
		{

			Academic_performance oldAcademPerf = GetAcademic_performance().FirstOrDefault(z => z.ID_Academic_performance == request.Id);
			oldAcademPerf.Date = request.newDate;
			await _academic_performance.UpdateAcademic_performance(oldAcademPerf);
			var str = request.Id + " " + request.newDate;
			return Json(str);
		}

		/// <summary>
		/// Изменяет оценку за рейтинг контроль
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("/Exams/ChangePoints/")]
		[Consumes("application/json")]
		public async Task<IActionResult> ChangePoints([FromBody] IdAndPoints request)
		{

			RatingControl oldRating = GetRatingControls().FirstOrDefault(z => z.ID_RatingControl == request.Id);
			oldRating.Points = request.newPoints;
			await _ratingControl.UpdateRatingControl(oldRating);
			var str = request.Id + " " + request.newPoints;
			return Json(str);
		}


		/// <summary>
		/// Создает новый рейтинг для студента если небыло
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("/Exams/NewRating/")]
		[Consumes("application/json")]
		public async Task<IActionResult> NewRating([FromBody] newRat request)
		{

			RatingControl newRating = new RatingControl()
			{
				ID_Student = request.IdStudent,
				ID_Subject = request.IdSubject,
				Semester = request.Semester,
				RatingNumber = request.NumberRating,
				Points = request.newPoints,
			};
			await _ratingControl.AddRatingControl(newRating);
			var str = newRating;
			return Json(str);
		}


	}
}
