using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Application.DTO;

namespace PrestigeRentals.Application.Services.Interfaces
{
    public interface IVehicleFilterService
    {
        Task<VehicleFilterOptionsDto> GetFilterOptionsAsync(CancellationToken cancellationToken);
    }
}
