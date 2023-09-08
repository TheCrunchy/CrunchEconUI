using CrunchEconUI.Services;
using Microsoft.AspNetCore.Components;

namespace CrunchEconUI.Shared
{
    public partial class MainLayout
    {
        [Inject] IHttpContextAccessor Accessor { get; set; }
        public AuthenticatedUserService UserService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            UserService = Accessor.HttpContext.RequestServices.GetService<AuthenticatedUserService>();
        }
    }
}
