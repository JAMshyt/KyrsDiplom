using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using recordBook.Models;
using recordBook.Repositories;
using recordBook.RInterface;
using System.Text.Json.Serialization;

namespace recordBook
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		public void ConfigureServices(IServiceCollection services)
		{
			var builder = WebApplication.CreateBuilder();

			string? connection = Configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<Context>(options =>
			options.UseSqlServer(connection));
			services.AddControllersWithViews();
			services.AddControllersWithViews().AddJsonOptions(x =>
			x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);


			services.AddScoped<IStudent, StudentRepositories>();
			services.AddScoped<IGroup, GroupRepositories>();
			services.AddScoped<ISubject, SubjectRepositories>();
			services.AddScoped<IKind_of_work, Kind_of_workRepositories>();
			services.AddScoped<IDepartment_worker, Department_workerRepositories>();
			services.AddScoped<IAcademic_performance, Academic_performanceRepositories>();
			services.AddScoped<IAttendance, AttendanceRepositories>();
			services.AddScoped<IDepartment_worker_Academic_performance, Department_worker_Academic_performanceRepositories>();
			services.AddScoped<IGroup_Subject, Group_SubjectRepositories>();
			services.AddScoped<ILogins, LoginsRepositories>();
			services.AddScoped<ICurator, CuratorRepositories>();
			services.AddScoped<IRatingControl, RatingControlRepositories>();
			services.AddScoped<ILoginsStudent, LoginsStudentRepositories>();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options => options.LoginPath = "/Authorization");

			services.AddAuthorization();
		}
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Authorization}/{id?}");
			});
		}
	}
}
