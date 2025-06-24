using Microsoft.Extensions.Logging;
using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Application.Services.Interfaces.Repositories;
using PrestigeRentals.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Services
{
    /// <summary>
    /// Service class for managing order operations like creation, retrieval, cancellation, etc.
    /// Implements the IOrderService interface.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<OrderService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/>.
        /// </summary>
        /// <param name="orderRepository">Service for accessing order data.</param>
        /// <param name="vehicleRepository">Service for accessing vehicle data.</param>
        /// <param name="emailService">Service for sending transactional emails.</param>
        /// <param name="userRepository">Service for accessing user data.</param>
        /// <param name="logger">Logger instance for tracking operations.</param>

        public OrderService(IOrderRepository orderRepository, IVehicleRepository vehicleRepository, IEmailService emailService, IUserRepository userRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _vehicleRepository = vehicleRepository;
            _emailService = emailService;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new order from the provided request and saves it to the database.
        /// </summary>
        /// <param name="createOrderRequest">The request containing order details.</param>
        /// <returns>A DTO containing the created order's details.</returns>
        public async Task<OrderDTO> CreateOrder(CreateOrderRequest createOrderRequest)
        {
            var vehicle = await _vehicleRepository.GetVehicleById(createOrderRequest.VehicleId);
            if (vehicle == null)
                throw new ArgumentException("Vehicle not found");

            if (!vehicle.Available)
                throw new InvalidOperationException("Vehicle is not available.");

            var duration = (createOrderRequest.EndTime - createOrderRequest.StartTime).TotalDays;
            if (duration <= 0)
                throw new ArgumentException("End time must be after start time.");

            var hasOverlap = await _orderRepository.AnyAsync(o =>
                o.VehicleId == createOrderRequest.VehicleId &&
                !o.IsCancelled &&
                (
                    (createOrderRequest.StartTime >= o.StartTime && createOrderRequest.StartTime < o.EndTime) ||
                    (createOrderRequest.EndTime > o.StartTime && createOrderRequest.EndTime <= o.EndTime) ||
                    (createOrderRequest.StartTime <= o.StartTime && createOrderRequest.EndTime >= o.EndTime)
                ));

            if (hasOverlap)
                throw new InvalidOperationException("The vehicle is already booked for the selected time period.");

            var totalCost = (decimal)duration * vehicle.PricePerDay;
            string bookingReference = $"PR-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

            var startTime = DateTime.SpecifyKind(createOrderRequest.StartTime, DateTimeKind.Utc);
            var endTime = DateTime.SpecifyKind(createOrderRequest.EndTime, DateTimeKind.Utc);

            var qrData = $"BookingRef:{bookingReference};VehicleId:{createOrderRequest.VehicleId};Start:{startTime:O};End:{endTime:O}";
            var qrBytes = GenerateQrCodeBytes(qrData); 
            var base64Qr = Convert.ToBase64String(qrBytes);
            var base64ImageSrc = $"data:image/png;base64,{base64Qr}";

            var order = new Order
            {
                UserId = createOrderRequest.UserId,
                VehicleId = createOrderRequest.VehicleId,
                StartTime = startTime,
                EndTime = endTime,
                PricePerDay = vehicle.PricePerDay,
                TotalCost = totalCost,
                BookingReference = bookingReference,
                QrCodeData = qrData,
                QrCodeBase64Image = base64ImageSrc,
                IsUsed = false
            };

            await _orderRepository.AddAsync(order);
            await _vehicleRepository.UpdateAsync(vehicle);

            var user = await _userRepository.GetUserById(createOrderRequest.UserId);
            if (user == null || string.IsNullOrEmpty(user.Email))
                throw new InvalidOperationException("User email not found");

            await _emailService.SendQrCodeEmailAsync(createOrderRequest.Email, bookingReference, qrData);

            Console.WriteLine($"[BACKEND] QR Data: {qrData}");
            _logger.LogInformation($"[BACKEND] QR Data: {qrData}");

            return new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                VehicleId = order.VehicleId,
                StartTime = startTime,
                EndTime = endTime,
                PricePerDay = order.PricePerDay,
                TotalCost = order.TotalCost,
                BookingReference = bookingReference,
                QrCodeData = qrData,
                QrCodeBase64Image = base64ImageSrc,
                IsUsed = false
            };
        }

        /// <summary>
        /// Retrieves all orders from the repository and returns them as a list of DTOs.
        /// </summary>
        /// <returns>A collection of order DTOs.</returns>
        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {
            // Retrieve all orders from the repository
            IEnumerable<Order> orders = await _orderRepository.GetAllAsync();
            List<OrderDTO> orderDTOs = new List<OrderDTO>();

            // Convert each order entity into an OrderDTO
            foreach (var order in orders)
            {
                orderDTOs.Add(new OrderDTO
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    VehicleId = order.VehicleId,
                    StartTime = order.StartTime,
                    EndTime = order.EndTime,
                    PricePerDay = order.PricePerDay,
                    TotalCost = order.TotalCost,
                    IsCancelled = order.IsCancelled
                });
            }

            // Return the collection of order DTOs
            return orderDTOs;
        }

        /// <summary>
        /// Retrieves an order by its ID and returns it as a DTO.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The order DTO if found, otherwise null.</returns>
        public async Task<OrderDTO> GetOrderById(long id)
        {
            // Retrieve the order by its ID
            Order order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return null;

            // Return the order details as a DTO
            return new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                VehicleId = order.VehicleId,
                StartTime = order.StartTime,
                EndTime = order.EndTime,
                PricePerDay = order.PricePerDay,
                TotalCost = order.TotalCost,
                IsCancelled = order.IsCancelled
            };
        }

        /// <summary>
        /// Cancels an order by setting its IsCancelled flag to true and saving the changes.
        /// </summary>
        /// <param name="id">The ID of the order to cancel.</param>
        /// <returns>True if the order was successfully cancelled, otherwise false.</returns>
        public async Task<bool> CancelOrder(long id)
        {
            // Retrieve the order by its ID
            Order order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return false;

            // Set the order as cancelled
            order.IsCancelled = true;

            // Update the order in the repository
            await _orderRepository.UpdateAsync(order);
            var vehicle = await _vehicleRepository.GetVehicleById(order.VehicleId);
            if (vehicle != null)
            {
                vehicle.Available = true;
                await _vehicleRepository.UpdateAsync(vehicle);
            }

            // Return true indicating the order was successfully cancelled
            return true;
        }

        /// <summary>
        /// Retrieves all orders associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A collection of order DTOs belonging to the specified user.</returns>
        public async Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(long userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            var orderDTOs = orders.Select(o => new OrderDTO
            {
                Id = o.Id,
                UserId = o.UserId,
                VehicleId = o.VehicleId,
                StartTime = o.StartTime,
                PricePerDay = o.PricePerDay,
                TotalCost = o.TotalCost,
                EndTime = o.EndTime
            });

            return orderDTOs;
        }

        /// <summary>
        /// Generates a QR code as a PNG byte array from the provided string data.
        /// </summary>
        /// <param name="data">The string to encode in the QR code.</param>
        /// <returns>A byte array representing the generated QR code in PNG format.</returns>
        private byte[] GenerateQrCodeBytes(string data)
        {
            using (var qrGenerator = new QRCoder.QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(data, QRCoder.QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCoder.PngByteQRCode(qrCodeData))
            {
                return qrCode.GetGraphic(20);
            }
        }
    }
}
