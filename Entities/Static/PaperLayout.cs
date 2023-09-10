using System.ComponentModel.DataAnnotations;

namespace Entities.Static
{
    public enum PaperLayout
    {
        [Display(Name = "BISE Layout")]
        Bise = 1,
        [Display(Name = "PEC Layout")]
        Pec,
        [Display(Name = "PPSC Layout")]
        Ppsc,

    }
    public static class PaperLayouts
    {
        [Display(Name ="BISE Layout")]
        public static int BISELayout { get; set; } = 1;
        [Display(Name = "PEC Layout")]
        public static int PECLayout { get; set; } = 2;
        [Display(Name = "PPSC Layout")]
        public static int PPSCLayout { get; set; } = 3;
    }
}
