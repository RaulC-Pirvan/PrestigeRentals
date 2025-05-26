using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDTO> CreateReview(CreateReviewRequest request);
        Task<IEnumerable<ReviewDTO>> GetAllReviews();
        Task<ReviewDTO> GetReviewById(long id);
        Task<IEnumerable<ReviewDTO>> GetReviewsByUserIdAsync(long userId);
        Task<List<ReviewDTO>> GetReviewsByVehicleIdAsync(long vehicleId);
    }
}
