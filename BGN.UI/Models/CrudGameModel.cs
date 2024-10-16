using BGN.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BGN.UI.Models
{
    public class CrudGameModel : CrudModel
    {

        public Game? Game { get; set; }
        public IFormFile? CoverPhoto { get; set; }
    }
}
