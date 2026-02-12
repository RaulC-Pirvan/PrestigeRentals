using System;

namespace PrestigeRentals.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when a vehicle with the specified ID is not found in the system.
    /// </summary>
    public class VehicleNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleNotFoundException"/> class with a specified vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle that was not found.</param>
        public VehicleNotFoundException(long vehicleId) : base($"Vehicle with ID {vehicleId} was not found.") { }
    }

    /// <summary>
    /// Exception thrown when vehicle options for the specified vehicle ID are not found.
    /// </summary>
    public class VehicleOptionsNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleOptionsNotFoundException"/> class with a specified vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle whose options were not found.</param>
        public VehicleOptionsNotFoundException(long vehicleId) : base($"Vehicle options for vehicle with ID {vehicleId} were not found.") { }
    }

    /// <summary>
    /// Exception thrown when an attempt is made to activate a vehicle that is already active.
    /// </summary>
    public class VehicleAlreadyActiveException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleAlreadyActiveException"/> class with a specified vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle that is already active.</param>
        public VehicleAlreadyActiveException(long vehicleId) : base($"Vehicle with ID {vehicleId} is already active.") { }
    }

    /// <summary>
    /// Exception thrown when an attempt is made to deactivate a vehicle that is already deactivated.
    /// </summary>
    public class VehicleAlreadyDeactivatedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleAlreadyDeactivatedException"/> class with a specified vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle that is already deactivated or not found.</param>
        public VehicleAlreadyDeactivatedException(long vehicleId) : base($"Vehicle with ID {vehicleId} is already deactivated or not found.") { }
    }

    /// <summary>
    /// Exception thrown when an attempt to update a vehicle with the specified ID fails.
    /// </summary>
    public class VehicleUpdateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleUpdateException"/> class with a specified vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle that failed to be updated.</param>
        public VehicleUpdateException(long vehicleId) : base($"Failed to update vehicle with ID {vehicleId}") { }
    }

    /// <summary>
    /// Exception thrown when a vehicle with the specified ID does not have a photo.
    /// </summary>
    public class VehiclePhotoNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclePhotoNotFoundException"/> class with a specified vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle that does not have a photo.</param>
        public VehiclePhotoNotFoundException(long vehicleId) : base($"Vehicle with ID {vehicleId} has no photo.") { }
    }
}
