using TrybeHotel.Models;
using Microsoft.EntityFrameworkCore;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Desenvolva o endpoint GET /hotel
        public IEnumerable<HotelDto> GetHotels()
        {
            return _context.Hotels
            .Include(h => h.City)
            .Select(hotel => new HotelDto
            {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = hotel.CityId,
                CityName = hotel.City != null ? hotel.City.Name : null,
                State = hotel.City!.State
            })
            .ToList();
        }

        // 5. Desenvolva o endpoint POST /hotel
        public HotelDto AddHotel(Hotel hotel)
        {
            //     var newHotel = new Hotel
            // {
            //     Name = name,
            //     Address = address,
            //     CityId = cityId
            // };

            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            // Fetch the city name using the navigation property (assuming CityId is a valid foreign key)
            var cityInfo = _context.Cities
                .Where(city => city.CityId == hotel.CityId)
                .Select(city => new CityDto
                {
                    Name = city.Name,
                    State = city.State
                })
                .FirstOrDefault();

            return new HotelDto
            {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = hotel.CityId,
                CityName = cityInfo!.Name,
                State = cityInfo.State
            };
        }
    }
}