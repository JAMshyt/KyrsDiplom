using recordBook.Models;

namespace recordBook.RInterface
{
	public interface IAcademic_performance
	{
		Task AddAcademic_performance(Academic_performance academic_performance);
		Task DeleteAcademic_performance(Academic_performance academic_performance);
		Task UpdateAcademic_performance(Academic_performance academic_performance);
		Task<Academic_performance?> GetAcademic_performance(Academic_performance academic_performance);
		Task<Academic_performance?> GetAcademic_performancebyID(int Id);
		IQueryable<Academic_performance> GetAllAcademic_performance();
	}
}
