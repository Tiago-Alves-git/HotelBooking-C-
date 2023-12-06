using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]

    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(Policy = "Client")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert)
        {
            try
            {
                //Pega o Token do User Email

                var token = HttpContext.User.Identity as ClaimsIdentity;
                var email = token?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                // Chama o método na repository para adicionar a reserva
                var newBooking = _repository.Add(bookingInsert, email!);

                // Retorna a resposta de sucesso
                return Created("", newBooking);
            }
            catch (GuestQuantityExceedsCapacityException)
            {
                // Retorna a resposta de erro quando a quantidade de hóspedes é maior do que a capacidade do quarto
                return BadRequest(new { message = "Guest quantity over room capacity" });
            }
        }


        [HttpGet("{Bookingid}")]
        [Authorize(Policy = "Client")]
        // [Authorize(Policy = "Admin")]

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


        public IActionResult GetBooking(int Bookingid)
        {


            // Obtém o email da pessoa usuária pelo token
            var token = HttpContext.User.Identity as ClaimsIdentity;
            var email = token?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            // Chama o método na repository para obter a reserva
            var bookingResponse = _repository.GetBooking(Bookingid, email!);

            // Retorna a resposta de sucesso
            if (bookingResponse == null)
            {
                return Unauthorized();
            }

            return Ok(bookingResponse);


        }
    }
}