using System.ComponentModel.DataAnnotations;

namespace recordBook.Models.ViewModels
{
    public class AddStudentViewModel
    {

        [Required(ErrorMessage = "Введите фамилию")]
        public string Surname { get; set; }



        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }


		public string? Patronymic { get; set; }


        public int ID_Group { get; set; }


		public IEnumerable<Group>? Groups { get; set; }

        public bool studentAdded { get; set; }

    }
}
