﻿using System.ComponentModel.DataAnnotations;

namespace recordBook.Models.ViewModels
{
	public class AccountViewModel
	{
		public string Surname { get; set; }
		public string Name { get; set; }
		public string? Patronymic { get; set; }
		public Group? Group { get; set; }
		public string Email { get; set; }
		public decimal Phone { get; set; }
		public string? Photo { get; set; }


		public bool AdminWatching { get; set; }
	}
}
