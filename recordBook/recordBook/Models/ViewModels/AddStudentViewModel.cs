using System.ComponentModel.DataAnnotations;

namespace recordBook.Models.ViewModels
{
    public class AddStudentViewModel
    {
		#region данные студента
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

		#region данные логина
		[Required(ErrorMessage = "Введите логин")]
		[StringLength(20, ErrorMessage = "Длинна логина должна быть больше 3 но меньше 20", MinimumLength = 3)]
		public string Login { get; set; }

		[Required(ErrorMessage = "Введите пароль")]
		[StringLength(20, ErrorMessage = "Длинна пароля должна быть больше 3 но меньше 20", MinimumLength = 3)]
		public string Password { get; set; }
		public string Email { get; set; }
		#endregion
	}
}
