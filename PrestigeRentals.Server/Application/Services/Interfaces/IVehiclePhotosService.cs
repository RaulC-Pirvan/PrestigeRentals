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
        Task<ActionResult<List<VehiclePhotos>>> GetVehiclePhotos(int vehicleId);
    }
}
