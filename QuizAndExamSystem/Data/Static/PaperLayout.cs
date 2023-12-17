using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Data.Static
{
    public enum PaperLayout
    {
        [Display(Name = "One Column")]
        OneColumn = 1,
        [Display(Name = "Two Column")]
        TwoColumn,
        [Display(Name = "Three Column")]
        ThreeColumn,

    }

    public enum PaperVersion
    {
        [Display(Name ="Single Version")]
        Single = 1,
        [Display(Name = "Four Versions (Stars)")]
        FourVersion,
        
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
