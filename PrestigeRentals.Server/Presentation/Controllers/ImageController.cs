using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Infrastructure.Persistence;
using System.Drawing;
using ZXing;
using ZXing.Windows.Compatibility;

namespace PrestigeRentals.Presentation.Controllers
{
    [ApiController]
    [Route("api/image")]

    /// <summary>
    /// Controller to manage image uploads, retrievals, and QR code validations for users and vehicles.
    /// </summary>
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

        /// <summary>
        /// Uploads a profile image for a specified user, replacing any existing image.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="image">The image file to upload.</param>
        /// <returns>Uploaded filename or error.</returns>
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

        /// <summary>
        /// Uploads a main image for a specified vehicle, replacing any existing main image.
        /// </summary>
        /// <param name="vehicleId">Vehicle identifier.</param>
        /// <param name="image">The image file to upload.</param>
        /// <returns>Uploaded filename or error.</returns>
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

        /// <summary>
        /// Uploads an ID card image for a user, replacing existing ID card image.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="image">The ID card image file.</param>
        /// <returns>Uploaded filename or error.</returns>
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


        /// <summary>
        /// Uploads an image for a vehicle (non-main).
        /// </summary>
        /// <param name="vehicleId">Vehicle identifier.</param>
        /// <param name="image">The image file.</param>
        /// <returns>Uploaded filename or error.</returns>
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

        /// <summary>
        /// Validates a QR code image to verify booking access.
        /// </summary>
        /// <param name="qrImage">QR code image file.</param>
        /// <returns>Validation result with success status and message.</returns>
        [HttpPost("booking/validate-qrcode")]
        public async Task<IActionResult> ValidateQrCode(IFormFile qrImage)
        {
            if (qrImage == null || qrImage.Length == 0)
                return BadRequest("QR Image is missing.");

            try
            {
                var tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + Path.GetExtension(qrImage.FileName));
                await using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    await qrImage.CopyToAsync(stream);
                }

                var qrText = DecodeQrCode(tempPath);
                qrText = qrText?.Trim();

                Console.WriteLine($"[DEBUG] Cod QR extras: {qrText}");

                var bookingRef = ExtractBookingReference(qrText);

                Console.WriteLine($"[DEBUG] BookingReference extras: {bookingRef}");

                if (string.IsNullOrEmpty(bookingRef))
                    return BadRequest(new { isValid = false, message = "Formatul codului QR este invalid." });

                // Caută comanda doar pe BookingReference
                var booking = await _dbContext.Orders
                  .FirstOrDefaultAsync(o => o.BookingReference == bookingRef);

                if (booking == null)
                    return Ok(new { isValid = false, message = "Comanda nu a fost găsită sau nu este activă în acest moment." });

                // Marchează comanda ca folosită
                booking.IsUsed = true;
                await _dbContext.SaveChangesAsync();

                return Ok(new { isValid = true, message = "Acces validat cu succes." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { isValid = false, message = "A apărut o eroare internă.", error = ex.Message });
            }
        }

        // === GET ===

        /// <summary>
        /// Retrieves the profile image of a user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>User image file or not found/error status.</returns>
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

        /// <summary>
        /// Retrieves the main image of a vehicle.
        /// </summary>
        /// <param name="vehicleId">Vehicle identifier.</param>
        /// <returns>Vehicle main image or not found/error status.</returns>
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

        /// <summary>
        /// Retrieves all image filenames for a vehicle.
        /// </summary>
        /// <param name="vehicleId">Vehicle identifier.</param>
        /// <returns>List of image filenames or not found/error status.</returns>
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

        /// <summary>
        /// Retrieves a specific vehicle image file by filename.
        /// </summary>
        /// <param name="fileName">Filename of the vehicle image.</param>
        /// <returns>Image file or error/not found status.</returns>
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

        /// <summary>
        /// Sets the default profile image for a user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Success message or error.</returns>
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

        /// <summary>
        /// Deletes all files in the specified directory.
        /// </summary>
        /// <param name="path">Directory path to clear.</param>
        private void ClearDirectory(string path)
        {
            foreach (var file in Directory.GetFiles(path))
                System.IO.File.Delete(file);
        }

        /// <summary>
        /// Returns MIME content type based on file extension.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>MIME type string.</returns>
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

        /// <summary>
        /// Decodes the QR code text from an image file.
        /// </summary>
        /// <param name="imagePath">Path to the image containing QR code.</param>
        /// <returns>Decoded QR code text or null if not found.</returns>
        private string DecodeQrCode(string imagePath)
        {
            var reader = new BarcodeReader(); // din ZXing.Windows.Compatibility
            using var bitmap = (Bitmap)Image.FromFile(imagePath);
            var result = reader.Decode(bitmap);
            return result?.Text;
        }

        /// <summary>
        /// Extracts the booking reference from decoded QR code text.
        /// </summary>
        /// <param name="qrText">Decoded QR code text.</param>
        /// <returns>Booking reference string or null if not found.</returns>
        private string ExtractBookingReference(string qrText)
        {
            var match = System.Text.RegularExpressions.Regex.Match(qrText, @"BookingRef:(PR-[A-Z0-9]+)");
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}
