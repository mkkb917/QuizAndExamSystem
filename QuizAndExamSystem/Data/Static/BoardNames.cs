using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.Static
{
    public enum BoardNames
    {
       
        [Display(Name = "BISE Lahore")]
        Lahore,
        [Display(Name = "BISE Rawalpindi")]
        Rawalpindi,
        [Display(Name = "BISE Bahawalpur")]
        Bahawalpur,
        [Display(Name = "BISE DG Khan")]
        D_G_Khan,
        [Display(Name = "BISE Faisalabad")]
        Faisalabad,
        [Display(Name = "BISE Gujranwala")]
        Gujranwala,
        [Display(Name = "BISE Multan")]
        Multan,
        [Display(Name = "BISE Sargodha")]
        Sargodha
    }


}
