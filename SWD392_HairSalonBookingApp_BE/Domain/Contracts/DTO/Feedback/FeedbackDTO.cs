using Domain.Contracts.DTO.Booking;

namespace Domain.Contracts.DTO.Feedback
{
    public class FeedbackDTO
    {
        public Guid BookingId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}
