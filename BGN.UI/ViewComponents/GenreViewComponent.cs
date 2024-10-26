﻿using Microsoft.AspNetCore.Mvc;
using BGN.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using BGN.Services.Abstractions;
using BGN.Services;
using BGN.Services.Abstractions.FilterModels;

namespace BGN.UI.ViewComponents
{
    public class GenreViewComponent : ViewComponent
    {
        private readonly IServiceManager _serviceManager;

        public GenreViewComponent(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager; 
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genres = await _serviceManager.GameService.GetAllGenresAsync(); // Fetch genres from database
            return View(genres);  // Pass the genres to the view
        }

    }
}