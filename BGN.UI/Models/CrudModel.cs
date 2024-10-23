using BGN.Shared;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BGN.UI.Models
{
    public class CrudModel
    {
        [ValidateNever]
        public required PersonDto CurrentUser { get; set; }
    }
}
