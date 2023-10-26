using Microsoft.AspNetCore.Hosting;

namespace ExamSystem
{
    public static class WC
    {
        //namespace for UploadCategoryEnums
        public static string UploadEnumPath = "ExamSystem.Data.Static";

        // images path
        public static string ProfileImagePath = @"\images\profile\";
        public static string LogoPath =  @"\images\school\";
        public static string NotificationPath = @"\images\notification\";
        public static string QrCodePath = @"\images\qrcodes\";

        // pdf file paths
        public static string PaperPathPDF = @"\files\Paper\";
        public static string UploadPaperPathPDF = @"\files\upload\pastpapers\";
        public static string UploadNotesPathPDF = @"\files\upload\notes\";
        public static string UploadBooksPathPDF = @"\files\upload\Books\";
        public static string UploadSyllabusPathPDF = @"\files\upload\syllabus\";
        public static string UploadDecorate = @"\files\upload\decorate\";
        public static string UploadEvents = @"\files\upload\events\";
        public static string UploadManage = @"\files\upload\manage\";
        public static string PDF_SED_Path = @"\files\SED\";
        public static string PDF_Book_Path = @"\files\Books\";
        public static string PdfUploads = @"\files\uploads\";

        //paths for font style file
        public static string Noori_Nastaleeq = @"\css\Noori_Nastaleeq.ttf";
             

    }
}
