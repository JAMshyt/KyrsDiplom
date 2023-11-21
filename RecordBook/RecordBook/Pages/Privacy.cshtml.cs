using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RecordBook.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ViewData["TimeStamp"] = DateTime.Now.ToString("d", new CultureInfo("ru-RUS"));
        }
    }
}