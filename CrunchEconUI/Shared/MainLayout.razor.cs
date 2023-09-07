﻿using CrunchEconUI.Services;
using Microsoft.AspNetCore.Components;

namespace CrunchEconUI.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public AuthenticatedUserService UserService { get; set; }
    }
}