using Domain.Entities;

namespace Application.Repositories
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        Task<Feedback> GetFeedbackById(Guid id);
    }
}
