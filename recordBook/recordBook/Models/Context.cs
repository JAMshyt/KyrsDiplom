using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
namespace recordBook.Models
{
	public class Context : DbContext
	{
		public DbSet<Student> Student { get; set; }
		public DbSet<Group> Group { get; set; }
		public DbSet<Subject> Subject { get; set; }
		public DbSet<Kind_of_work> Kind_of_work { get; set; }
		public DbSet<Department_worker> Department_worker { get; set; }
		public DbSet<Academic_performance> Academic_performance { get; set; }
		public DbSet<Attendance> Attendance { get; set; }
		public DbSet<Department_worker_Academic_performance> Department_worker_Academic_performance { get; set; }
		public DbSet<Group_Subject> Group_Subject { get; set; }
		public DbSet<Logins> Logins { get; set; }
		public DbSet<Curator> Curator{ get; set; }


		public Context(DbContextOptions<Context> options)
			: base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Student>()
		.ToTable(tb => tb.HasTrigger("afterStudentAdd"));
		}
	}
}
