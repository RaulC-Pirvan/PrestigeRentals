using System;
using AutoMapper;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Helpers
{
    /// <summary>
    /// Defines the AutoMapper profile for mapping between domain entities and data transfer objects (DTOs).
    /// This class configures mappings to transform data between the application and domain layers.
    /// </summary>
    public partial class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// Configures the mapping profiles for the application.
        /// </summary>
        public MappingProfile()
        {
            #region Vehicle
            /// <summary>
            /// Maps between the <see cref="Vehicle"/> entity and the <see cref="VehicleDTO"/> DTO.
            /// </summary>
            CreateMap<Vehicle, VehicleDTO>();

            /// <summary>
            /// Maps between the <see cref="VehicleRequest"/> and the <see cref="Vehicle"/> entity.
            /// </summary>
            CreateMap<VehicleRequest, Vehicle>();

            /// <summary>
            /// Maps between the <see cref="VehicleOptionsDTO"/> DTO and the <see cref="VehicleOptions"/> entity.
            /// </summary>
            CreateMap<VehicleOptionsDTO, VehicleOptions>();
            #endregion
        }
    }
}
