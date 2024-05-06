using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace recordBook.Models.ViewModels
{
    public class AddStudentViewModel
    {
		#region данные студента
		[Required(ErrorMessage = "Введите номер зачетной книжки")]
		public int NumberBook { get; set; }
		[Required(ErrorMessage = "Введите фамилию")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }
		public string? Patronymic { get; set; }
        public int ID_Group { get; set; }
		#endregion

		public IEnumerable<Group>? Groups { get; set; }
		public bool studentAdded { get; set; }
		public bool loginUnique { get; set; }
		public bool EmailUnique { get; set; }
		public bool PhoneUnique { get; set; }
		public bool BookError { get; set; }

		#region данные логина
		//[Required(ErrorMessage = "Введите логин")]
		//[StringLength(20, ErrorMessage = "Длинна логина должна быть больше 3 но меньше 20", MinimumLength = 4)]
		public string? Login { get; set; }

		[Required(ErrorMessage = "Введите пароль")]
		[StringLength(20, ErrorMessage = "Длинна пароля должна быть больше 3 но меньше 20", MinimumLength = 4)]
		public string Password { get; set; }
		public string Email { get; set; }
		[Required(ErrorMessage ="Введите номер телефона")]
		[MinLength(5,ErrorMessage ="Слишком короткий")]
		[MaxLength(15, ErrorMessage = "Слишком длинный")]

		public string Phone { get; set; }
		#endregion
	}
}
