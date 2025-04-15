using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Exceptions
{
    public class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException(int vehicleId) : base($"Vehicle with ID {vehicleId} was not found.") { }
    }

    public class VehicleOptionsNotFoundException : Exception
    {
        public VehicleOptionsNotFoundException(int vehicleId) : base($"Vehicle options for vehicle with ID {vehicleId} were not found.") { }
    }

    public class VehicleAlreadyActiveException : Exception
    {
        public VehicleAlreadyActiveException(int vehicleId) : base($"Vehicle with ID {vehicleId} is already active.") { }
    }

    public class VehicleAlreadyDeactivatedException : Exception
    {
        public VehicleAlreadyDeactivatedException(int vehicleId) : base($"Vehicle with ID {vehicleId} is already deactivated or not found.") { }
    }

    public class VehicleUpdateException : Exception
    {
        public VehicleUpdateException(int vehicleId) : base($"Failed to update vehicle with ID {vehicleId}") { }
    }

    public class VehiclePhotoNotFoundException : Exception
    {
        public VehiclePhotoNotFoundException(int vehicleId) : base($"Vehicle with ID {vehicleId} has no photo.") { }
    }
}
