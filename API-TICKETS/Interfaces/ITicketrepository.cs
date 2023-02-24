using API_TICKETS.Models;

namespace API_TICKETS.Interfaces
{
    public interface ITicketrepository
    {

        Task<IEnumerable<dynamic>> GetAll(int pageNumber, int pageSize, DateTime? startDate, DateTime? endDate, string? status);  
        Task<int> Count();
        Task Create(Ticket ticket);
        Task<Ticket> getOne(int id);
        Task update(Ticket ticket);
        Task Delete(Ticket ticket);


    }
}
