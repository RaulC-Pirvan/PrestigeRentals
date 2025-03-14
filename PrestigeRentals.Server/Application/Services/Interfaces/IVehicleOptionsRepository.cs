using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Services.Interfaces
{
    public interface IVehicleOptionsRepository
    {
        Task AddVehicleOptions(VehicleOptions vehicleOptions);
    }
}
