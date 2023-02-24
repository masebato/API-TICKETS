using API_TICKETS.Controllers;
using API_TICKETS.Interfaces;
using API_TICKETS.Models;
using API_TICKETS.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API_TICKETS_TEST
{
    public class Tests
    {
        private Mock<ITicketrepository> _mockTicketRepository;
        private TicketController _ticketController;

        [SetUp]
        public void Setup()
        {
            _mockTicketRepository = new Mock<ITicketrepository>();
            _ticketController = new TicketController(_mockTicketRepository.Object);
        }

        [Test]
        public async Task Delete_ExistingTicket_ReturnsOk()
        {
          
            int id = 1;
            _mockTicketRepository.Setup(r => r.getOne(id)).ReturnsAsync(new Ticket());

          
            IActionResult result = await _ticketController.Delete(id);

          
            Assert.IsInstanceOf<ObjectResult>(result);
            var okResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
           
        }

        [Test]
        public async Task Delete_NonExistingTicket_ReturnsBadRequest()
        {
          
            int id = 1;
            _mockTicketRepository.Setup(r => r.getOne(id));

            IActionResult result = await _ticketController.Delete(id);

          
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual("ticket not found", badRequestResult.Value);
        }

        [Test]
        public async Task Update_ValidTicket_ReturnsOk()
        {
            // Arrange
            Ticket ticket = new Ticket { IdTicket = 1, Description = "Description 1", Status = "Open" };
            Ticket oTicket = new Ticket { IdTicket = 1, Description = "Description 1", Status = "Open" };
            _mockTicketRepository.Setup(r => r.getOne(ticket.IdTicket)).ReturnsAsync(oTicket);

            // Act
            IActionResult result = await _ticketController.update(ticket);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            _mockTicketRepository.Verify(r => r.update(oTicket), Times.Once);
        }

        [Test]
        public async Task Save_ValidTicket_ReturnsOk()
        {
            // Arrange
            Ticket ticket = new Ticket { IdTicket = 1, Description = "Description 1", Status = "Open" };

            // Act
            IActionResult result = await _ticketController.save(ticket);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.IsNotNull(okResult.Value);
            dynamic value = okResult.Value;
            _mockTicketRepository.Verify(r => r.Create(ticket), Times.Once);
        }

        [Test]
        public async Task GetTickets_Returns_OkResult_With_Response()
        {
            // Arrange
            var tickets = new List<Ticket>
            {
                new Ticket { IdTicket = 1, Description = "Ticket 1", DateCreate = new DateTime(2022, 1, 1), Status = "ABIERTO" },
                new Ticket { IdTicket = 2, Description = "Ticket 2", DateCreate = new DateTime(2022, 2, 1), Status = "CERRADO" },
                new Ticket { IdTicket = 3, Description = "Ticket 3", DateCreate = new DateTime(2022, 3, 1), Status = "ABIERTO" },
            };
            _mockTicketRepository.Setup(repo => repo.GetAll(It.IsAny<int>(), It.IsAny<int>(), null, null, null))
                .ReturnsAsync(tickets);

            // Act
            var result = await _ticketController.getTickets();

            // Assert
            var okResult = result.Result as OkObjectResult;
            var response = okResult.Value as Response;
            Assert.AreEqual(3, response.TotalCount);
        }




    }
}