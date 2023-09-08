using CrunchEconUI.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CrunchEconUI.Pages
{
    public class HostModel : PageModel
    {
        public UserInfo userInfo { get; private set; }
        // ... set the information up
    }
}
