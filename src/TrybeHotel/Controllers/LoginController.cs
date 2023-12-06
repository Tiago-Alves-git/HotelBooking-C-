using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using TrybeHotel.Services;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("login")]

    public class LoginController : Controller
    {

        private readonly IUserRepository _repository;

        private readonly TokenGenerator _tokenGenerator;
        public LoginController(IUserRepository repository, TokenGenerator token)
        {
            _repository = repository;
            _tokenGenerator = token;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto login)
        {
            try
            {
                // Chama o método na repository para autenticar o usuário
                var authenticatedUser = _repository.Login(login);

                // Gera o token com base no usuário autenticado
                var token = _tokenGenerator.Generate(authenticatedUser);

                // Retorna a resposta de sucesso com o token
                return Ok(new TokenDto { Token = token });
            }
            catch (InvalidCredentialsException)
            {
                // Retorna a resposta de erro quando as credenciais estão incorretas
                return Unauthorized(new { message = "Incorrect e-mail or password" });
            }
        }
    }
}