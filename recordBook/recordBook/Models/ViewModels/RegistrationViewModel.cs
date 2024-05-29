using System.ComponentModel.DataAnnotations;

namespace recordBook.Models.ViewModels
{
	public class RegistrationViewModel
	{
		[Required(ErrorMessage = "Введите почту")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Введите код из сообщения")]
		public string AcceptEmailCode { get; set; }


		[Required(ErrorMessage = "Введите логин")]
		[MinLength(3,ErrorMessage ="Слишком короткий логин")]
		[MaxLength(20, ErrorMessage = "Логин должен быть короче 20 символов")]
		public string Login { get; set; }


		[Required(ErrorMessage = "Введите фамилию")]
		public string Surname { get; set; }
		[Required(ErrorMessage = "Введите имя")]
		public string Name { get; set; }
		public string? Patronymic { get; set; }

		[Required(ErrorMessage = "Введите код из деканата")]
		public int ActivationCode { get; set; }


		public string? generatedCode { get; set; }


		public bool ErrorText_ActivationCode { get; set; }

		public bool ErrorText_SurnameName { get; set; }
		public bool ErrorText_StudentFioAndCode { get; set; }
		public bool ErrorText_Email { get; set; }
		public bool ErrorText_IsEmail { get; set; }

		public bool ErrorText_EmailCode { get; set; }
		public bool ErrorText_LoginOld { get; set; }
		public bool Succes_SendEmail { get; set; }
		public bool ErrorText_LoginExist { get; set; }

		public bool Succes { get; set; }
	}
}
