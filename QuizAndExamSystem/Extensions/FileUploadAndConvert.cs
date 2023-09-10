using Aspose.Pdf;
using Aspose.Pdf.Devices;
using ExamSystem.Data;

namespace ExamSystem.Extensions
{
    // extension class for uploading file and creating image of that file 
    // and upload to corresponding folder.
    public static class FileUploadAndConvert
    {
        public static string? UploadFileAndConvertToImage(IFormFileCollection files, string path)
        {
            if (files != null)
            {
                // create the parameters
                string fileUpload =  path;
                string fileName = Guid.NewGuid().ToString()[..5];
                string fileExtension = Path.GetExtension(files[0].FileName);

                // copy the file in directory   i.e    wwwroot/files/Book
                using (var fileStream = new FileStream(Path.Combine(fileUpload, fileName + fileExtension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                /// convert pdf to image section
                string imageExtension = ".jpeg";
                Resolution resolution = new Resolution(300);
                PngDevice pngDevice = new PngDevice(resolution);
                Document pdfDocument = new Document(fileUpload + fileName + fileExtension);

                // create image and copy it to system folder   wwwroot/files/Books
                using (var imageStream = new FileStream(Path.Combine(fileUpload, fileName + imageExtension), FileMode.Create))
                {
                    pngDevice.Process(pdfDocument.Pages[1], imageStream);
                    imageStream.Close();
                }
                return fileName;
            }
            else
            {
                return null;
            }
        }
    }
}
