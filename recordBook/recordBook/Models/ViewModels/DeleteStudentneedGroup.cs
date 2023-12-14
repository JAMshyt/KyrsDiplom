using System.ComponentModel.DataAnnotations;

namespace recordBook.Models.ViewModels
{
    public class DeleteStudentneedGroup
    {

        [Required(ErrorMessage = "Заполните фамилию")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Заполните имя")]
        public string Name { get; set; }
		public string Patronymic { get; set; }
        public int ID_Group { get; set; }


		public IEnumerable<Group> Groups { get; set; }

		public Group selectedGroup { get; set; }

    }
}
