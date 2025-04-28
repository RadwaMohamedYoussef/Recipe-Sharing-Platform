using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
namespace Recipe_Sharing_Platform.Helpers
{
    public static class ImageHelper
    {
        public static async Task<string> SaveResizedImageAsync(IFormFile file, string uploadPath)
        {
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(800, 600)); 
                await image.SaveAsync(filePath);
            }

            return fileName;
        }
    }
}
