using System;

namespace PrestigeRentals.Application.Requests
{
    /// <summary>
    /// Represents a request to upload or update a vehicle's photo.
    /// </summary>
    public class VehiclePhotoRequest
    {
        /// <summary>
        /// Gets or sets the image data for the vehicle's photo.
        /// This should be a base64-encoded string representing the image.
        /// </summary>
        public string ImageData { get; set; }
    }
}
