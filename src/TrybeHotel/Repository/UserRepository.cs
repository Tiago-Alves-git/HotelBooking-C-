using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password) ?? throw new InvalidCredentialsException();

            // Retorna o DTO do usuário autenticado
            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            };
        }
        public UserDto Add(UserDtoInsert user)
        {
            // Verifica se o e-mail já existe
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                throw new EmailAlreadyExistsException();
            }

            // Adiciona a pessoa usuária
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client" // Atributo UserType deve ser salvo com o valor client
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            // Retorna o DTO da pessoa usuária adicionada
            return new UserDto
            {
                UserId = newUser.UserId,
                Name = newUser.Name,
                Email = newUser.Email,
                UserType = newUser.UserType
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserDto> GetUsers()
        {
            return _context.Users
            .Select(user => new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            })
            .ToList();
        }

    }
}

public class EmailAlreadyExistsException : Exception { }
public class InvalidCredentialsException : System.Exception { }
