namespace recordBook.Models.ViewModels
{
	public class RegistrationViewModel
	{
		public string Email { get; set; }
		public string AcceptEmailCode{ get; set; }

		public string Login{ get; set; }
		public string Surname { get; set; }
		public string Name { get; set; }
		public string? Patronymic { get; set; }

		public string ActivationCode { get; set; }
	}
}
