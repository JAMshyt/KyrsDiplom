using System.ComponentModel.DataAnnotations;

namespace recordBook.Models
{
	public class Group
	{
		[Key]
		public int ID_Group { get; set; }
		public string Name_group { get; set; }
		public string Decoding { get; set; }
		public string Course { get; set; }
		public string Financing_source { get; set; }
		public int ID_Curator { get; set; }	
		public string? Graduating_department { get; set; }
	}
}
