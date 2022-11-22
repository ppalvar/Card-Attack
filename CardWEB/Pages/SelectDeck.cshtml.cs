using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class SelectDeckModel : PageModel
    {
        public int a{get;set;} = 0;
        public void OnGet()
        {
        }
    }
}
