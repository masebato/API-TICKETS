using API_TICKETS.Interfaces;
using API_TICKETS.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_TICKETS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketrepository _ticketRepository;

        public TicketController(ITicketrepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> getTickets(int pageNumber = 1, int pageSize = 10, DateTime? startDate = null, DateTime? endDate = null, string? status = null)
        {
            try
            {
             
                var totalCount = await _ticketRepository.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var tickets = await _ticketRepository.GetAll(pageNumber, pageSize, startDate, endDate, status);

                Response oResponse = new Response
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    Items = tickets
                };

                return Ok(oResponse);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> save([FromBody] Ticket ticket)
        {
            try
            {
                ticket.DateCreate = DateTime.Now;
                await _ticketRepository.Create(ticket);
                return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }

        }

        [HttpPut]
        public async Task<IActionResult> update([FromBody] Ticket ticket)
        {

            Ticket oTicket = await _ticketRepository.getOne(ticket.IdTicket);
            if (oTicket == null)
            {
                return BadRequest("ticket not found");
            }

            try
            {
                oTicket.Description = ticket.Description ?? oTicket.Description;
                oTicket.Status = ticket.Status ?? oTicket.Status;
                oTicket.DateUpdate = DateTime.Now;

                await _ticketRepository.update(oTicket);

                return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpDelete]
        [Route("delete/{idTicket:int}")]
        public async Task<IActionResult> Delete(int idTicket)
        {
            Ticket oTicket = await _ticketRepository.getOne(idTicket);

            if (oTicket == null)
            {
                return BadRequest("ticket not found");
            }

            try
            {
                await _ticketRepository.Delete(oTicket);
                return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

    }
}
