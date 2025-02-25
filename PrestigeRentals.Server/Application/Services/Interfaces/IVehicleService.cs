using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetAllVehicles();
        Task<ActionResult?> AddVehicle(CreateVehicleDTO createVehicleDTO);
    }
}
