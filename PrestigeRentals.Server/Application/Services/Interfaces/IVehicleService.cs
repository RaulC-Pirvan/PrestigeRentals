using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetAllVehicles(bool? onlyActive = false);
        Task<ActionResult?> AddVehicle(VehicleRequest vehicleRequest);
        Task<Vehicle> GetVehicleByID(int vehicleId);
        Task<bool> DeactivateVehicle(int vehicleId);
        Task<bool> ActivateVehicle(int vehicleId);
        Task<bool> DeleteVehicle(int vehicleId);
        Task<VehicleDTO> UpdateVehicle(int vehicleId, VehicleRequest vehicleRequest);
    }
}
