using System.ComponentModel.DataAnnotations;

namespace recordBook.Models
{
	public class Logins
	{
		[Key]
		public int ID_Login { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public decimal Phone { get; set; }
		public string? Salt { get; set; }
	}
}
