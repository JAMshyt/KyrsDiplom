using Microsoft.EntityFrameworkCore;
namespace recordBook.Models
{
	public class Context : DbContext
	{
		public DbSet<Student> Students { get; set; } 
		public Context(DbContextOptions<Context> options) 
			: base(options)
		{
			Database.EnsureCreated();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Student>().HasData(
					new Student { Id = 1, Name = "Tom", Surname = "as",Patronymic="asdasd",ID_Group=1 },
					new Student { Id = 2, Name = "Bob", Surname = "zx" ,Patronymic = "asdasd", ID_Group = 1 },
					new Student { Id = 3, Name = "Sam", Surname = "as" , Patronymic = "asdasd" , ID_Group = 1 }
			);
		}
	}
}
