﻿using System.ComponentModel.DataAnnotations;


namespace recordBook.Models
{
    public class Student
    {
        [Key]
        public int ID_Student { get; set; }


        [Required(ErrorMessage = "Enter this")]
        public string Surname { get; set; }


        [Required(ErrorMessage = "Заполните имя")]
        public string Name { get; set; }


		public string? Patronymic { get; set; }


        [Required(ErrorMessage = "Выберите группу")]
        public int ID_Group { get; set; }
		public int NumberOfBook { get; set; }

        public byte[]? Photo { get; set; }
	}
}
