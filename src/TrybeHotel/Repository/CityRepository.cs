using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Refatore o endpoint GET /city
        public IEnumerable<CityDto> GetCities()
        {
            return _context.Cities
            .Select(city => new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State
            })
            .ToList();
        }

        // 2. Refatore o endpoint POST /city
        public CityDto AddCity(City city)
        {
            var newCity = new City { Name = city.Name, State = city.State };
            _context.Cities.Add(newCity);
            _context.SaveChanges();

            return new CityDto
            {
                CityId = newCity.CityId,
                Name = newCity.Name,
                State = newCity.State
            };
        }

        // 3. Desenvolva o endpoint PUT /city
        public CityDto UpdateCity(City city)
        {
            var existingCity = _context.Cities.FirstOrDefault(c => c.CityId == city.CityId);
            if (existingCity != null)
            {
                existingCity.State = city.State;
                existingCity.Name = city.Name;
                _context.SaveChanges();
                return new CityDto
                {
                    CityId = existingCity.CityId,
                    Name = existingCity.Name,
                    State = existingCity.State
                };
            }
            return null!;
        }

    }
}