using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PrestigeRentals.Application.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for handling vehicle photo services, including uploading and retrieving vehicle photos.
    /// </summary>
    public interface IVehiclePhotosService
    {
        /// <summary>
        /// Asynchronously retrieves vehicle photos as a list of Base64 encoded strings for a given vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle for which photos are to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of Base64 encoded images.</returns>
        Task<ActionResult<List<string>>> GetVehiclePhotosAsBase64(long vehicleId);

        /// <summary>
        /// Asynchronously uploads a new vehicle photo for a given vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle for which the photo will be uploaded.</param>
        /// <param name="base64Image">The photo to be uploaded, represented as a Base64 encoded string.</param>
        /// <returns>A task representing the asynchronous operation, containing the vehicle photo details.</returns>
        Task<ActionResult<VehiclePhotos>> UploadVehiclePhoto(long vehicleId, string base64Image);
    }
}
