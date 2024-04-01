using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recordBook.Migrations
{
	/// <inheritdoc />
	public partial class InsertDatas2 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string students = "Student";
			migrationBuilder.Sql("SET IDENTITY_INSERT " + students + " ON");
			migrationBuilder.InsertData(
				table: students, // Имя вашей таблицы
				columns: new[] { "ID_Student", "Surname", "[Name]", "Patronymic", "ID_Group", "ID_Login" }, // Список столбцов
				values: new object[,]
				{
					{1,"Щербаков", "Захар", "Михайлович",1,6 },
					{2,"Новожиловм", "Илья", "Александрович",2,7 },
					{3, "Владимиров", "Артём", "Сергеевич",3,8},

				});
			migrationBuilder.Sql("SET IDENTITY_INSERT " + students + " OFF");
			
			string groups = "[Group]";
			migrationBuilder.Sql("SET IDENTITY_INSERT " + groups + " ON");
			migrationBuilder.InsertData(
				table: groups, // Имя вашей таблицы
				columns: new[] { "ID_Group", "Name_group", "Decoding", "Course" }, // Список столбцов
				values: new object[,]
				{
					{1,"ПКсп-120", "Программирование в компьютерных системах", 4 },
					{2,"ТО-123", "Техническое обслуживание и ремонт двигателей",1 },
					{3, "ПБ-122", "Пожарная безопасность",2},

				});
			migrationBuilder.Sql("SET IDENTITY_INSERT " + groups + " OFF");

			string workers = "Department_worker";
			migrationBuilder.Sql("SET IDENTITY_INSERT " + workers + " ON");
			migrationBuilder.InsertData(
				table: workers, // Имя вашей таблицы
				columns: new[] { "ID_Department_worker", "Surname", "[Name]", "Patronymic", "ID_Login" }, // Список столбцов
				values: new object[,]
				{
					{1,"Соседкин", "Глеб", "Захарович", 1 },
					{2,"Румянцев", "Артем", "Сергеевич",2 },
					{3, "Староверов", "Алексей", "Дмитриевич",3},
					{4,"Чебыкин", "Максим", "Дмитриевич",4 },
					{5, "Савиных", "Михаин", "Андреевич",5},

				});
			migrationBuilder.Sql("SET IDENTITY_INSERT " + workers + " OFF");

			string kindOfWorks = "Kind_of_work";
			migrationBuilder.Sql("SET IDENTITY_INSERT " + kindOfWorks + " ON");
			migrationBuilder.InsertData(
				table: kindOfWorks, // Имя вашей таблицы
				columns: new[] { "ID_Kind_of_work", "Title_of_kind" }, // Список столбцов
				values: new object[,]
				{
					{1,"Курсовая работа" },
					{2, "Экзамен"},
					{3, "Дифференцируемый зачет"},
					{4, "Практика"},

				});
			migrationBuilder.Sql("SET IDENTITY_INSERT " + kindOfWorks + " OFF");


			string subjects = "[Subject]";
			migrationBuilder.Sql("SET IDENTITY_INSERT " + subjects + " ON");
			migrationBuilder.InsertData(
				table: subjects, // Имя вашей таблицы
				columns: new[] { "ID_Subject", "Name_Subject" }, // Список столбцов
				values: new object[,]
				{
					{1,"Программирование" },
					{2, "Русский язык"},
					{3, "Математика"},
					{4, "Литература"},
					{5, "Музыка"},
				});
			migrationBuilder.Sql("SET IDENTITY_INSERT " + subjects + " OFF");

			string attendance = "Attendance";
			migrationBuilder.Sql("SET IDENTITY_INSERT " + attendance + " ON");
			migrationBuilder.InsertData(
				table: attendance, // Имя вашей таблицы
				columns: new[] { "ID_Attendance", "ID_Subject", "ID_Student", "Date_presence", "Precense" }, // Список столбцов
				values: new object[,]
				{
					{ 1,1,1,"1.12.2023",1 },
					{ 2,2,2,"1.12.2023",0 },
					{ 3,3,3,"1.12.2023",1 },
				});
			migrationBuilder.Sql("SET IDENTITY_INSERT " + attendance + " OFF");

			string academPerf = "Academic_performance";
			migrationBuilder.Sql("SET IDENTITY_INSERT " + academPerf + " ON");
			migrationBuilder.InsertData(
				table: academPerf, // Имя вашей таблицы
				columns: new[] { "ID_Academic_performance", "ID_Subject", "ID_Student", "ID_Kind_of_work", "[Date]", "Grade" }, // Список столбцов
				values: new object[,]
				{
					{ 1,1,1,2, "1.12.2023","Хорошо" },
					{ 2,2,2,2, "1.12.2023","Отлично" },
					{3, 3, 3, 2, "1.12.2023", "-"},
				});
			migrationBuilder.Sql("SET IDENTITY_INSERT " + academPerf + " OFF");

			string Group_Subject = "Group_Subject";
			migrationBuilder.Sql("SET IDENTITY_INSERT " + Group_Subject + " ON");
			migrationBuilder.InsertData(
				table: Group_Subject, // Имя вашей таблицы
				columns: new[] { "ID_Group_Subject", "ID_Group", "ID_Subject"}, // Список столбцов
				values: new object[,]
				{
					{ 1,1,1 },
					{ 2,2,2 },
					{3, 3, 3},
				});
			migrationBuilder.Sql("SET IDENTITY_INSERT " + Group_Subject + " OFF");

			string logs = "Logins";
			migrationBuilder.Sql("SET IDENTITY_INSERT " + logs + " ON");
			migrationBuilder.InsertData(
				table: logs, // Имя вашей таблицы
				columns: new[] { "ID_login", "Login", "Password", "Email" }, // Список столбцов
				values: new object[,]
				{
					{1, "teach1","pass1","teach1@gmail.com" },
					{2, "teach2","pass2","teach2@gmail.com" },
					{3, "teach3","pass3","teach3@gmail.com" },
					{4, "teach4","pass4","teach4@gmail.com" },
					{5, "teach5","pass5","teach5@gmail.com" },
					{6, "stud1","stt1","st1@gmail.com" },
					{7, "stud2","stt2","st2@gmail.com" },
					{8, "stud3","stt3","st3@gmail.com" },
				});
			migrationBuilder.Sql("SET IDENTITY_INSERT " + logs + " OFF");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
