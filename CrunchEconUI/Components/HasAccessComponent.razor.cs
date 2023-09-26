using CrunchEconUI.Models;
using CrunchEconUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CrunchEconUI.Components
{
    public partial class HasAccessComponent
    {
        [CascadingParameter]
        public AuthenticatedUserService User { get; set; }

        [Parameter]
        public int AccessLevel { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

    }
}
