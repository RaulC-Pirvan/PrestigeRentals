using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.Helpers
{
    public partial class MappingProfile:Profile
    {
        public MappingProfile()
        {
            #region Vehicle
            CreateMap<CreateVehicleDTO, Vehicle>();
            #endregion
        }
    }
}
