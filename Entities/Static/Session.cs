using System.ComponentModel.DataAnnotations;

namespace Entities.Static
{
    public enum Session
    {
        
        [Display(Name = "Morning")]
        Morning,
        [Display(Name = "Evening")]
        Evening
    }
}
