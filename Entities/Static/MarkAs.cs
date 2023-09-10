using System.ComponentModel.DataAnnotations;

namespace Entities.Static
{
    public enum MarkAs
    {
        
        [Display(Name = "Important")]
        Important ,
        [Display(Name = "Very Important")]
        Very_Important,
        [Display(Name = "Not Important")]
        Out_Dated,
    }
}
