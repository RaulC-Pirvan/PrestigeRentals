using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces
{
    public interface IVehicleOptionsService
    {
        Task<VehicleOptions> CreateVehicleOptions(int vehicleId, VehicleOptionsRequest vehicleOptionsRequest);
        Task<VehicleOptions> GetOptionsByVehicleId(int vehicleId);
        Task<VehicleOptions> UpdateVehicleOptions(int vehicleId, VehicleOptionsRequest vehicleOptionsRequest);
        Task<bool> DeleteVehicleOptions(int vehicleId);
    }
}
