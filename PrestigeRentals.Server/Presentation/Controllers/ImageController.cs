using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PrestigeRentals.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PrestigeRentals.Presentation.Controllers
{
    [ApiController]
    [Route("api/image")]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _dbContext;

        public ImageController(IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            _env = env;
            _dbContext = dbContext;
        }

        private string RootImages => Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "images");

        // === POST ===

        [HttpPost("user/{userId}")]
        public async Task<IActionResult> UploadUserImage(string userId, IFormFile image)
        {
            var path = Path.Combine(RootImages, "user", userId);
            Directory.CreateDirectory(path);
            ClearDirectory(path);

            var fileName = "profile" + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(path, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            if (long.TryParse(userId, out var userIdLong))
            {
                var userDetails = await _dbContext.UsersDetails.FirstOrDefaultAsync(u => u.UserID == userIdLong);
                if (userDetails != null)
                {
                    userDetails.ProfileImageFileName = fileName;
                    await _dbContext.SaveChangesAsync();
                }
                else
                    return NotFound($"User with id {userId} not found.");
            }
            else
                return BadRequest("Invalid userId");
            return Ok(new { fileName });
        }

        [HttpPost("vehicle/{vehicleId}/main")]
        public async Task<IActionResult> UploadMainVehicleImage(string vehicleId, IFormFile image)
        {
            var path = Path.Combine(RootImages, "vehicle", vehicleId, "main");
            Directory.CreateDirectory(path);
            ClearDirectory(path);

            var fileName = "main" + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(path, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            return Ok(new { fileName });
        }

        [HttpPost("vehicle/{vehicleId}")]
        public async Task<IActionResult> UploadVehicleImage(string vehicleId, IFormFile image)
        {
            var path = Path.Combine(RootImages, "vehicle", vehicleId);
            Directory.CreateDirectory(path);

            var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(path, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            return Ok(new { fileName });
        }

        // === GET ===

        [HttpGet("user/{userId}")]
        public IActionResult GetUserImage(string userId)
        {
            var path = Path.Combine(RootImages, "user", userId);
            var file = Directory.GetFiles(path).FirstOrDefault();
            if (file == null) return NotFound("No profile image found.");

            return File(System.IO.File.OpenRead(file), GetContentType(file));
        }

        [HttpGet("vehicle/{vehicleId}/main")]
        public IActionResult GetMainVehicleImage(string vehicleId)
        {
            var path = Path.Combine(RootImages, "vehicle", vehicleId, "main");
            var file = Directory.GetFiles(path).FirstOrDefault();
            if (file == null) return NotFound("No main image found.");

            return File(System.IO.File.OpenRead(file), GetContentType(file));
        }

        [HttpGet("vehicle/{vehicleId}")]
        public IActionResult GetAllVehicleImages(string vehicleId)
        {
            var path = Path.Combine(RootImages, "vehicle", vehicleId);
            if (!Directory.Exists(path)) return NotFound();

            var files = Directory.GetFiles(path)
                .Select(Path.GetFileName)
                .ToList();

            return Ok(files);
        }

        // === Helpers ===

        private void ClearDirectory(string path)
        {
            foreach (var file in Directory.GetFiles(path))
                System.IO.File.Delete(file);
        }

        private string GetContentType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}
