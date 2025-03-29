using Lab_1.Controllers;
using Lab_1.Models;
using System.Linq;

namespace Lab_1.Extentions
{
    public static class ImagesHelper
    {
        private static IWebHostEnvironment _environment;
        private static  string _imagePath;

        static ImagesHelper()
        {
           
        }

        public static void Initialize(IWebHostEnvironment webHostEnvironment)
        {
            _environment = webHostEnvironment;
            _imagePath = $"{_environment.WebRootPath}/images/students";
        }

        public static string GetPathImage(string name)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            var fileExtension = Path.GetExtension(name).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return "Invalid image format.";
            }
            string uploadDir = Path.Combine(_imagePath);
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            //upload image in server 
            var fileName = $"{Guid.NewGuid()}{fileExtension}";

            var filePath = Path.Combine(_imagePath, fileName);

            return filePath;
        }

        public static string Delete(string name)
        {

                    var existingPhotoPath = Path.Combine(_imagePath, name);
                    if (System.IO.File.Exists(existingPhotoPath))
                    {
                        try
                        {
                            System.IO.File.Delete(existingPhotoPath);
                        }
                        catch (Exception ex)
                        {
                            return $"An unexpected error occurred: {ex.Message}";
                        }
                    }


                return $"Deleted images done";
          
        }
    }
}
