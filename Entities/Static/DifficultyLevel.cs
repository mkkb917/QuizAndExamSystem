using System.ComponentModel.DataAnnotations;

namespace Entities.Static
{
    public enum DifficultyLevel
    {
       
        [Display(Name ="Very Easy")]
        Very_Easy,
        [Display(Name = "Easy")]
        Easy,
        [Display(Name = "Moderate")]
        Moderate,
        [Display(Name = "Hard")]
        Hard,
        [Display(Name = "Very Hard")]
        Very_Hard,
        
    }
}
