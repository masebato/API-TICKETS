namespace API_TICKETS.Repository;
using API_TICKETS.Data;
using API_TICKETS.Interfaces;
using API_TICKETS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class TicketRepository : ITicketrepository
{
    private readonly TicketapiContext _dbContext;
    private readonly DbSet<Ticket> _dbSet;

    public TicketRepository(TicketapiContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<Ticket>();
    }


    public async Task<IEnumerable<dynamic>> GetAll(int pageNumber, int pageSize, DateTime? startDate, DateTime? endDate, string? status)
    {
        
        var skipAmount = pageSize * (pageNumber - 1);

        var query = _dbSet.Include(p => p.IdUserNavigation).AsQueryable();

        if (startDate.HasValue)
        {
            query= query.Where(t => t.DateCreate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query= query.Where(t => t.DateCreate <= endDate.Value);
        }

        if (!string.IsNullOrEmpty(status))
        {
            query= query.Where(t => t.Status == status);
        }

        
        return await query.OrderByDescending(t=> t.DateCreate).Skip(skipAmount).Take(pageSize).ToListAsync();

    }

    public async Task<int> Count()
    {
        return await _dbSet.CountAsync();
    }
    public async Task Create(Ticket ticket)
    {
        await _dbSet.AddAsync(ticket);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Ticket> getOne(int id)
    {
        Ticket currentTicket = await _dbSet.FindAsync(id);

        currentTicket = _dbSet.Include(p => p.IdUserNavigation).Where(p => p.IdTicket == id).FirstOrDefault();

        return currentTicket;
    }

    public async Task update(Ticket ticket)
    {
        _dbSet.Attach(ticket);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Ticket ticket)
    {

        var setUser = _dbContext.Set<User>();
        setUser.Remove(ticket.IdUserNavigation);
        _dbSet.Remove(ticket);
        await _dbContext.SaveChangesAsync();
    }
}
