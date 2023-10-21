using CrunchEconModels.Models;
using CrunchEconUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CrunchEconUI.Components
{
    public partial class NewShipListingComponent
    {
        [Parameter]
        public AuthenticatedUserService User { get; set; }
        [Inject]
        DialogService DialogService { get; set; }
        private long maxFileSize = 1024 * 6000;

        [Inject]
        public IWebHostEnvironment Environment { get; set; }

        public String SelectedFilePath;
        public List<String> AdditionalImages = new List<String>();

        private ShipListing NewListing = new ShipListing();

        public async Task Submit()
        {

        }
            private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {

                    var trustedFileNameForFileStorage = file.Name;
                    var path = Path.Combine(Environment.WebRootPath, "Textures/Ships/",
                            trustedFileNameForFileStorage);

                    await using FileStream fs = new(path, FileMode.Create);
                    await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
                    SelectedFilePath = $"Textures/Ships/{file.Name}";
                    await InvokeAsync(StateHasChanged);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
        private async Task LoadFilesAdditional(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {
                try
                {
                    var trustedFileNameForFileStorage = file.Name;
                    var path = Path.Combine(Environment.WebRootPath, "Textures/Ships/",
                            trustedFileNameForFileStorage);

                    await using FileStream fs = new(path, FileMode.Create);
                    await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
                    AdditionalImages.Add($"Textures/Ships/{file.Name}");

                }
                catch (Exception ex)
                {
                    return;
                }
            }
            await InvokeAsync(StateHasChanged);
        }
    }
}

