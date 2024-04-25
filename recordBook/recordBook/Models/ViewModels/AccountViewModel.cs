using System.ComponentModel.DataAnnotations;

namespace recordBook.Models.ViewModels
{
	public class AccountViewModel
	{
		public string Surname { get; set; }
		public string Name { get; set; }
		public string? Patronymic { get; set; }
		public Group? Group { get; set; } //группа студента
		public string Email { get; set; }
		public decimal Phone { get; set; }
		public string? Photo { get; set; }

		public string? Graduating_department { get; set; }//выпускающая кафедра студента

		public string Institute_title { get; set; }//название института работника деканата
		public string Job_title { get; set; }//должность работника кфедры

		public bool AdminWatching { get; set; }//проверка на то смотрит ли профиль его владелец или аминистратор просматривает чужой профиль
	}
}
