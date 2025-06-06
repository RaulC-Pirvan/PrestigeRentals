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
            try
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
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                        return NotFound($"User with id {userId} not found.");
                }
                else
                    return BadRequest("Invalid userId");

                return Ok(new { fileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("vehicle/{vehicleId}/main")]
        public async Task<IActionResult> UploadMainVehicleImage(string vehicleId, IFormFile image)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("user/{userId}/idcard")]
        public async Task<IActionResult> UploadUserIdCardImage(string userId, IFormFile image)
        {
            try
            {
                var path = Path.Combine(RootImages, "user", userId, "idcard");
                Directory.CreateDirectory(path);
                ClearDirectory(path);

                var fileName = "idcard" + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(path, fileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream);

                return Ok(new { fileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("vehicle/{vehicleId}")]
        public async Task<IActionResult> UploadVehicleImage(string vehicleId, IFormFile image)
        {
            try
            {
                var path = Path.Combine(RootImages, "vehicle", vehicleId);
                Directory.CreateDirectory(path);

                var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(path, fileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream);

                return Ok(new { fileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // === GET ===

        [HttpGet("user/{userId}")]
        public IActionResult GetUserImage(string userId)
        {
            try
            {
                var path = Path.Combine(RootImages, "user", userId);
                var file = Directory.GetFiles(path).FirstOrDefault();
                if (file == null) return NotFound("No profile image found.");

                return File(System.IO.File.OpenRead(file), GetContentType(file));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("vehicle/{vehicleId}/main")]
        public IActionResult GetMainVehicleImage(string vehicleId)
        {
            try
            {
                var path = Path.Combine(RootImages, "vehicle", vehicleId, "main");
                var file = Directory.GetFiles(path).FirstOrDefault();
                if (file == null) return NotFound("No main image found.");

                return File(System.IO.File.OpenRead(file), GetContentType(file));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("vehicle/{vehicleId}")]
        public IActionResult GetAllVehicleImages(string vehicleId)
        {
            try
            {
                var path = Path.Combine(RootImages, "vehicle", vehicleId);
                if (!Directory.Exists(path)) return NotFound();

                var files = Directory.GetFiles(path)
                    .Select(Path.GetFileName)
                    .ToList();

                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("vehicle/file/{fileName}")]
        public IActionResult GetVehicleImageByFileName(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return BadRequest("Filename is required");

                var vehiclePath = Path.Combine(RootImages, "vehicle");

                var files = Directory.GetFiles(vehiclePath, fileName, SearchOption.AllDirectories);

                if (files.Length == 0)
                    return NotFound();

                var filePath = files[0];
                var contentType = GetContentType(filePath);
                return File(System.IO.File.OpenRead(filePath), contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("user/{userId}/default")]
        public async Task<IActionResult> SetDefaultUserImage(string userId)
        {
            try
            {
                if (!long.TryParse(userId, out var userIdLong))
                    return BadRequest("Invalid userId");

                var userImagePath = Path.Combine(RootImages, "user", userId);
                Directory.CreateDirectory(userImagePath);

                ClearDirectory(userImagePath);

                var defaultImagePath = Path.Combine(RootImages, "default", "default-profile-account-unknown-icon-black-silhouette-free-vector.jpg");

                if (!System.IO.File.Exists(defaultImagePath))
                    return NotFound("Default profile image not found.");

                var destFileName = "profile" + Path.GetExtension(defaultImagePath);
                var destFilePath = Path.Combine(userImagePath, destFileName);

                System.IO.File.Copy(defaultImagePath, destFilePath, overwrite: true);

                var userDetails = await _dbContext.UsersDetails.FirstOrDefaultAsync(u => u.UserID == userIdLong);
                if (userDetails == null)
                    return NotFound($"User with id {userId} not found.");

                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "Default image set successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
