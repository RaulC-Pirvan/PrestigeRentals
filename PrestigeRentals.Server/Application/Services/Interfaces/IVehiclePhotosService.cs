using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Interfaces
{
    public interface IVehiclePhotosService
    {
        Task<ActionResult<List<string>>> GetVehiclePhotosAsBase64(int vehicleId);
        Task<ActionResult<VehiclePhotos>> UploadVehiclePhoto(int vehicleId, string base64Image);
    }
}
