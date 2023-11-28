using Spire.Pdf.Graphics;
using Spire.Pdf;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;

namespace ExamSystem.Extensions
{
    // extension class for uploading file and creating image of that file 
    // and upload to corresponding folder.
    public static class FileUploadAndConvert
    {
        public static string? UploadFileAndConvertToImage(IFormFileCollection files, string path,string name)
        {
            if (files != null)
            {
                // create the parameters
                
                string fileExtension = Path.GetExtension(files[0].FileName);

                // copy the file in directory   i.e    wwwroot/files/Book
                using (var fileStream = new FileStream(Path.Combine(path, name + fileExtension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                if (fileExtension == ".pdf")
                {
                    //code ok and working with FreeSpire library
                    //Create a PdfDocument instance
                    PdfDocument pdf = new PdfDocument();

                    //Load a sample PDF document
                    using (var stream = files[0].OpenReadStream())
                    {
                        pdf.LoadFromStream(stream);
                    }

                    //Convert the first page to an image and set the image Dpi
                    Image image = pdf.SaveAsImage(0, PdfImageType.Bitmap, 250, 250);

                    //Save the image as a JPEG file
                    image.Save(Path.Combine(path, string.Format("{0}.jpeg", name)));
                }
                else if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
                {
                    var oldfile = Path.Combine(path, name + fileExtension);

                    try
                    {
                        // delete old file if exist
                        if (System.IO.File.Exists(oldfile))
                        {
                            System.IO.File.Delete(oldfile);
                        }
                        //copy new file to local storage
                        using (var filePicstream = new FileStream(Path.Combine(path, name + fileExtension), FileMode.Create))
                        {
                            files[0].CopyTo(filePicstream);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (e.g., log, show user-friendly message)
                        Console.WriteLine("Error: " + ex.Message);
                        // Optionally, rollback any changes made
                    }
                }
                return name;
            }
            else
            {
                return null;
            }
        }

    }
}
