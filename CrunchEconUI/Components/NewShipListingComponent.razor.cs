using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CrunchEconUI.Components
{
    public partial class NewShipListingComponent
    {
        private List<IBrowserFile> loadedFiles = new();
        private long maxFileSize = 1024 * 6000;

        [Inject]
        public IWebHostEnvironment Environment { get; set; }

        public String SelectedFilePath;


        private async Task LoadFiles(InputFileChangeEventArgs e)
        {

            loadedFiles.Clear();

            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    loadedFiles.Add(file);

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

    }
}

