using System.ComponentModel.DataAnnotations;


namespace recordBook.Models
{
	public class Curator
	{
		[Key]
		public int ID_Curator { get; set; }


		[Required(ErrorMessage = "Enter this")]
		public string Surname { get; set; }


		[Required(ErrorMessage = "Заполните имя")]
		public string Name { get; set; }


		public string? Patronymic { get; set; }


		public int ID_Login { get; set; }
		public byte[]? Photo { get; set; }

	}
}
