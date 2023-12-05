using Microsoft.AspNetCore.Hosting;

namespace ExamSystem
{
    public static class WC
    {
        // log file path 
        public static string logfilePath = @"\logs\";
        //namespace for UploadCategoryEnums
        public static string UploadEnumPath = "ExamSystem.Data.Static";

        // images path
        public static string ProfileImagePath = @"\images\profile\";
        public static string GradeImagePath = @"\images\grade\";
        public static string SubjectImagePath = @"\images\subject\";
        public static string TopicImagePath = @"\images\topic\";
        public static string LogoPath =  @"\images\school\";
        public static string NotificationPath = @"\images\notification\";
        public static string QrCodePath = @"\images\qrcodes\";

        // pdf file paths
        public static string PDF_SED_Path = @"\files\SED\";
        public static string PDF_Book_Path = @"\files\Books\";
        public static string PaperPathPDF = @"\files\Paper\";
        // upload folders for Corner Controller
        public static string UploadPastPaper = @"\files\upload\pastpapers\";
        public static string UploadNotes = @"\files\upload\notes\";
        public static string UploadBooks = @"\files\upload\books\";
        public static string UploadSyllabus = @"\files\upload\syllabus\";
        public static string UploadDecorate = @"\files\upload\decorate\";
        public static string UploadEvents = @"\files\upload\events\";
        public static string UploadManage = @"\files\upload\manage\";
        public static string UploadCalender = @"\files\upload\calender\";
        
        public static string PdfUploads = @"\files\uploads\";

        //paths for font style file
        public static string Noori_Nastaleeq = @"\css\Noori_Nastaleeq.ttf";
             

    }
}
