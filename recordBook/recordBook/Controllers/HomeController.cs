using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using recordBook.Models;
using recordBook.Models.ViewModels;
using recordBook.RInterface;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using static recordBook.Controllers.StudentController;
using MailKit;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace recordBook.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private readonly IStudent _student;
		private readonly IDepartment_worker _department_worker;
		private readonly ILogins _logins;
		private readonly ICurator _curator;
		private readonly IGroup _group;
		private readonly ILoginsStudent _loginsStudent;


		public HomeController(ILogger<HomeController> logger, IStudent student,
			IDepartment_worker department_worker, ILogins logins, ICurator curator,
			IGroup group, ILoginsStudent loginsStudent
			)
		{
			_logger = logger;
			_student = student;
			_department_worker = department_worker;
			_logins = logins;
			_curator = curator;
			_group = group;
			_loginsStudent = loginsStudent;
		}

		#region Get таблиц
		public List<Student> GetStudents()
		{
			var students = _student.GetAllStudent().ToList();
			return students;
		}

		public List<Department_worker> GetDepartment_worker()
		{
			var dep_wor = _department_worker.GetAllDepartment_worker().ToList();
			return dep_wor;
		}

		public List<Curator> GetCurators()
		{
			var curators = _curator.GetAllCurator().ToList();
			return curators;
		}

		public List<Group> GetGroups()
		{
			var group = _group.GetAllGroup().ToList();
			return group;
		}

		public List<Logins> GetLogins()
		{
			var logins = _logins.GetAllLogins().ToList();
			return logins;
		}

		public List<LoginsStudent> GetLoginsStudents()
		{
			var logs = _loginsStudent.GetAllLoginsStudent().ToList();
			return logs;
		}
		#endregion

		#region страницы

		public ViewResult Authorization()
		{
			AuthorizationViewModel user = new AuthorizationViewModel() { ErrorText = false };
			return View(user);
		}

		public ViewResult Registration()
		{
			RegistrationViewModel user = new RegistrationViewModel()
			{
				ErrorText_ActivationCode = false,
				ErrorText_SurnameName = false,
				ErrorText_StudentFioAndCode = false,
				ErrorText_Email = false,
				ErrorText_EmailCode = false,
				Succes_SendEmail = false,
				Succes = false,
				ErrorText_LoginOld = false,
				ErrorText_LoginExist = false,
			};
			return View(user);
		}

		/// <summary>
		/// Авторизацияя пользоваьеля.
		/// Ищет введенный логин в бд, хеширует введенный пароль и сравниввает с хешем в бд
		/// </summary>
		/// <param name="user">логин с паролем</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult> Authorization(AuthorizationViewModel user)
		{
			if (ModelState.IsValid)
			{
				string CreateSalt()
				{
					byte[] salt = new byte[16];
					using (var rng = RandomNumberGenerator.Create())
					{
						rng.GetBytes(salt);
					}
					return Convert.ToBase64String(salt).Substring(0, 16);
				}

				string HashPassword(string password, string salt)
				{
					byte[] saltBytes = Convert.FromBase64String(salt);
					byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

					byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
					Array.Copy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
					Array.Copy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

					using (var sha512 = SHA512.Create())
					{
						byte[] hashedBytes = sha512.ComputeHash(combinedBytes);
						return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
					}
				}
				Logins? loginWordker = GetLogins().FirstOrDefault(q => q.Login == user.Login);
				LoginsStudent? loginStudent = GetLoginsStudents().FirstOrDefault(q => q.Login == user.Login);

				if (loginWordker is null & loginStudent is null)
				{
					user.ErrorText = true;
					return View(user);
				}
				else if (loginWordker != null)
				{
					string salt = CreateSalt();
					string hashedPassword = HashPassword(user.Password, loginWordker.Salt);

					if (hashedPassword != loginWordker.Password)
					{
						user.ErrorText = true;
						return View(user);
					}
					else
					{
						user.ErrorText = false;

						try
						{
							Department_worker? adm = GetDepartment_worker().FirstOrDefault(q => q.ID_Login == loginWordker.ID_Login);
							string patronymic = adm.Patronymic == null ? "" : adm.Patronymic;

							var claims = new List<Claim> {
								new Claim(ClaimTypes.NameIdentifier, loginWordker.Login),
								new Claim(ClaimTypes.Email, loginWordker.Email),
								new Claim(ClaimTypes.Name, adm.Name),
								new Claim(ClaimTypes.Surname, adm.Surname),
								new Claim(ClaimTypes.GivenName, patronymic),
								new Claim(ClaimTypes.MobilePhone, Convert.ToString(loginWordker.Phone)),
								new Claim(ClaimTypes.Role, "Adm"),
								new Claim(ClaimTypes.SerialNumber, Convert.ToString(adm.ID_Department_worker)),
							};
							ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
							await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
							return RedirectToAction("AccountInfo", "Account");
						}
						catch
						{
							Curator? curator = GetCurators().FirstOrDefault(q => q.ID_Login == loginWordker.ID_Login);

							List<Group>? curGroups = GetGroups().Where(q => q.ID_Curator == curator.ID_Curator).ToList();
							string patronymic = curator.Patronymic == null ? "" : curator.Patronymic;

							var claims = new List<Claim> {
								new Claim(ClaimTypes.NameIdentifier, loginWordker.Login),
								new Claim(ClaimTypes.Email, loginWordker.Email),
								new Claim(ClaimTypes.Name, curator.Name),
								new Claim(ClaimTypes.Surname, curator.Surname),
								new Claim(ClaimTypes.GivenName, patronymic),
								new Claim(ClaimTypes.MobilePhone, Convert.ToString(loginWordker.Phone)),
								new Claim(ClaimTypes.Role, "Curator"),
								new Claim(ClaimTypes.SerialNumber, Convert.ToString(curator.ID_Curator))
							};
							claims.AddRange(curGroups.Select(group => new Claim(ClaimTypes.GroupSid, group.ID_Group.ToString())));


							ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
							await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
							return RedirectToAction("AccountInfo", "Account");
						}

					}
				}
				else
				{
					string salt = CreateSalt();
					string hashedPassword = HashPassword(user.Password, loginStudent.Salt);

					if (hashedPassword != loginStudent.Password)
					{
						user.ErrorText = true;
						return View(user);
					}
					else
					{
						user.ErrorText = false;

						Student? student = GetStudents().FirstOrDefault(q => q.NumberOfBook == loginStudent.Number_RecordBook);
						string patronymic = student.Patronymic == null ? "" : student.Patronymic;

						var claims = new List<Claim> {
							new Claim(ClaimTypes.NameIdentifier, loginStudent.Login),
							new Claim(ClaimTypes.Email, loginStudent.Email),
							new Claim(ClaimTypes.Name, student.Name),
							new Claim(ClaimTypes.Surname, student.Surname),
							new Claim(ClaimTypes.GivenName, patronymic),
							new Claim(ClaimTypes.MobilePhone, Convert.ToString(loginStudent.Phone)),
							new Claim(ClaimTypes.Role, "Student"),
							new Claim(ClaimTypes.GroupSid, Convert.ToString(student.ID_Group)),
							new Claim(ClaimTypes.SerialNumber, Convert.ToString(student.ID_Student)),
							};
						ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
						await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
						return RedirectToAction("AccountInfo", "Account");
					}
				}
			}
			else return View(user);
		}

		/// <summary>
		/// Регистрация студента, сравнивает введеные ФИО и зачетку
		/// Проверка почты
		/// Создание логина
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult> Registration(RegistrationViewModel user)
		{

			RegistrationViewModel model = new RegistrationViewModel() { ErrorText_ActivationCode = false, 
				ErrorText_SurnameName = false, ErrorText_StudentFioAndCode = false, ErrorText_Email = false,
				ErrorText_IsEmail = false,
				ErrorText_EmailCode = false,
				ErrorText_LoginOld = false,
				Succes_SendEmail = false,
				ErrorText_LoginExist = false,
				Succes = false
			};

			Student? stuBook = GetStudents().FirstOrDefault(x => x.NumberOfBook == Convert.ToInt32(user.ActivationCode));
			Student? stuSurName = GetStudents().FirstOrDefault(x => x.Surname == user.Surname && x.Name == user.Name && x.Patronymic == user.Patronymic);


			LoginsStudent? login = GetLoginsStudents().FirstOrDefault(x => x.Number_RecordBook == user.ActivationCode);
			if (stuBook == null)
			{
				model.ErrorText_ActivationCode = true;
			}
			if (stuSurName == null)
			{
				model.ErrorText_SurnameName = true;
			}
			if (stuBook != stuSurName)
			{
				model.ErrorText_StudentFioAndCode = true;
			}

			if (login != null & login?.Email != user.Email)
			{
				model.ErrorText_Email = true;
			}

			string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
			if (!Regex.IsMatch(user.Email, pattern, RegexOptions.IgnoreCase))
			{
				model.ErrorText_IsEmail = true;
			}

			LoginsStudent? l = GetLoginsStudents().FirstOrDefault(q => q.Login == user.Login);
			if (login?.Login != null)
			{
				model.ErrorText_LoginOld = true;
			}
			else if (l != null & !user.Login.IsNullOrEmpty())
			{
				model.ErrorText_LoginExist = true;
			}

			if (user.generatedCode != user.AcceptEmailCode)
			{
				model.ErrorText_EmailCode = true;
			}

			if (model.ErrorText_ActivationCode == true ||
				model.ErrorText_SurnameName == true ||
				model.ErrorText_StudentFioAndCode == true ||
				model.ErrorText_Email == true ||
				model.ErrorText_EmailCode == true ||
				model.ErrorText_LoginOld == true ||
				!ModelState.IsValid)
			{
				return View(model);
			}
			else
			{
				LoginsStudent loginsStu = GetLoginsStudents().FirstOrDefault(q => q.Number_RecordBook == user.ActivationCode);
				loginsStu.Login = user.Login;
				await _loginsStudent.UpdateLoginStudent(loginsStu);
				model.Succes = true;
				return View(model);
			}

		}


		/// <summary>
		/// Проверка почты
		/// Создлается код, который отсылается по почте
		/// Сравнивается введеный код с тем что отослали
		/// </summary>
		/// <param name="Email"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("/Home/EmailSend/")]
		public async Task<IActionResult> EmailSend(string Email)
		{
			if (Email != null)
			{
				using var emailMessage = new MimeMessage();
				emailMessage.From.Add(new MailboxAddress("Электронная зачётка", "ElectrZachetka@gmail.com"));
				emailMessage.To.Add(new MailboxAddress("", Email));
				emailMessage.Subject = "Код подтверждения";

				const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
				var random = new Random();
				var Code = new string(Enumerable.Repeat(chars, 6)
					.Select(s => s[random.Next(s.Length)])
					.ToArray());


				emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
				{
					Text = "Код для подтверждения почты - " + Code
				};


				using (var client = new SmtpClient())
				{
					await client.ConnectAsync("smtp.gmail.com", 465, true);
					await client.AuthenticateAsync("ElectrZachetka@gmail.com", "bbxm qbnd hyro ovce");
					await client.SendAsync(emailMessage);
					await client.DisconnectAsync(true);
				}
				return Ok(Code);
			}
			return BadRequest("Email не указан");
		}


		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Authorization", "Home");
		}

		#endregion

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{

			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}