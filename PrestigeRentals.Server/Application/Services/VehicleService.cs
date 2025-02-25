using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;

namespace PrestigeRentals.Application.Services
{
    public class VehicleService(ApplicationDbContext dbContext, IMapper mapper) : IVehicleService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<List<Vehicle>> GetAllVehicles()
        {
            return await _dbContext.Vehicles.ToListAsync();
        }

        public async Task<ActionResult?> AddVehicle(CreateVehicleDTO createVehicleDTO)
        {
            Vehicle mappedVehicle = _mapper.Map<Vehicle>(createVehicleDTO);
            _dbContext.Vehicles.Add(mappedVehicle);
            await _dbContext.SaveChangesAsync();

            return null;
        }
    }
}
