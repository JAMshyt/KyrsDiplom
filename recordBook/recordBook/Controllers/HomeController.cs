using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
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
using static System.Runtime.InteropServices.JavaScript.JSType;
using static recordBook.Controllers.StudentController;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Text;

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

		public HomeController(ILogger<HomeController> logger, IStudent student,
			IDepartment_worker department_worker, ILogins logins, ICurator curator,
			IGroup group
			)
		{
			_logger = logger;
			_student = student;
			_department_worker = department_worker;
			_logins = logins;
			_curator = curator;
			_group = group;
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

		#endregion

		#region страницы

		public ViewResult Authorization()
		{
			AuthorizationViewModel user = new AuthorizationViewModel() { ErrorText = false };
			return View(user);
		}

		public ViewResult Registration()
		{
			RegistrationViewModel user = new RegistrationViewModel();
			return View(user);
		}


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
				Logins? login = GetLogins().FirstOrDefault(q => q.Login == user.Login);

				string salt = CreateSalt();
				string hashedPassword = HashPassword(user.Password, login.Salt);

				if (login is null || hashedPassword != login.Password)
				{
					user.ErrorText = true;
					return View(user);
				}
				else
				{
					user.ErrorText = false;
					try
					{
						Student? student = GetStudents().FirstOrDefault(q => q.ID_Login == login.ID_Login);

						var claims = new List<Claim> {
							new Claim(ClaimTypes.NameIdentifier, login.Login),
							new Claim(ClaimTypes.Email, login.Email),
							new Claim(ClaimTypes.Name, student.Name),
							new Claim(ClaimTypes.Surname, student.Surname),
							new Claim(ClaimTypes.GivenName, student.Patronymic),
							new Claim(ClaimTypes.MobilePhone, Convert.ToString(login.Phone)),
							new Claim(ClaimTypes.Role, "Student"),
							new Claim(ClaimTypes.GroupSid, Convert.ToString(student.ID_Group)),
							new Claim(ClaimTypes.SerialNumber, Convert.ToString(student.ID_Student)),
						};
						ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
						await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
						return RedirectToAction("AccountInfo", "Account");
					}
					catch
					{
						try
						{
							Department_worker? adm = GetDepartment_worker().FirstOrDefault(q => q.ID_Login == login.ID_Login);

							var claims = new List<Claim> {
								new Claim(ClaimTypes.NameIdentifier, login.Login),
								new Claim(ClaimTypes.Email, login.Email),
								new Claim(ClaimTypes.Name, adm.Name),
								new Claim(ClaimTypes.Surname, adm.Surname),
								new Claim(ClaimTypes.GivenName, adm.Patronymic),
								new Claim(ClaimTypes.MobilePhone, Convert.ToString(login.Phone)),
								new Claim(ClaimTypes.Role, "Adm"),
								new Claim(ClaimTypes.SerialNumber, Convert.ToString(adm.ID_Department_worker)),
							};
							ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
							await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
							return RedirectToAction("AccountInfo", "Account");
						}
						catch
						{
							Curator? curator = GetCurators().FirstOrDefault(q => q.ID_Login == login.ID_Login);

							List<Group>? curGroups = GetGroups().Where(q => q.ID_Curator == curator.ID_Curator).ToList();

							var claims = new List<Claim> {
								new Claim(ClaimTypes.NameIdentifier, login.Login),
								new Claim(ClaimTypes.Email, login.Email),
								new Claim(ClaimTypes.Name, curator.Name),
								new Claim(ClaimTypes.Surname, curator.Surname),
								new Claim(ClaimTypes.GivenName, curator.Patronymic),
								new Claim(ClaimTypes.MobilePhone, Convert.ToString(login.Phone)),
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
			}
			else return View(user);
		}

		[HttpPost]
		public async Task<ActionResult> Registration(AuthorizationViewModel user)
		{ 
			return View(user);
		}

			#endregion

			[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{

			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}


	}
}