using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            // Obter a pessoa usuária pelo email no token
            var user = _context.Users.SingleOrDefault(u => u.Email == email);

            // Obter o quarto pelo RoomId
            var room = _context.Rooms.SingleOrDefault(r => r.RoomId == booking.RoomId);

            // Verificar se há room

            if (room == null)
            {
                // Handle the case where the room is not found, e.g., throw an exception or return an error response
                // For now, I'll throw an exception as an example
                throw new RoomNotFoundException();
            }

            // Obter Hotel e City

            var hotel = _context.Hotels.SingleOrDefault(r => r.HotelId == room!.HotelId);

            var city = _context.Cities.SingleOrDefault(c => c.CityId == hotel!.CityId);

            // Verificar se a quantidade de hóspedes é maior do que a capacidade do quarto

            if (booking.GuestQuant > room.Capacity)
            {
                throw new GuestQuantityExceedsCapacityException();
            }

            // Nova reserva

            var newBooking = new Booking
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                RoomId = booking.RoomId,
                UserId = user!.UserId
            };

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            // DTO de resposta
            var bookingResponse = new BookingResponse
            {
                BookingId = newBooking.BookingId,
                CheckIn = newBooking.CheckIn,
                CheckOut = newBooking.CheckOut,
                GuestQuant = newBooking.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = hotel!.HotelId,
                        Name = hotel!.Name,
                        Address = hotel!.Address,
                        CityId = city!.CityId,
                        CityName = city!.Name,
                        State = city!.State
                    }
                }
            };

            return bookingResponse;
        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            // Obter a pessoa usuária pelo email no token
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            // Obter a reserva por seu ID
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            // Obter o quarto pelo RoomId
            var room = _context.Rooms.SingleOrDefault(r => r.RoomId == booking!.RoomId);
            // Obter Hotel e City
            var hotel = _context.Hotels.SingleOrDefault(r => r.HotelId == room!.HotelId);
            var city = _context.Cities.SingleOrDefault(c => c.CityId == hotel!.CityId);
            // DTO de resposta
            var bookingResponse = _context.Bookings
            .Where(b => b.UserId == user!.UserId && b.BookingId == bookingId)
                .Select(b => new BookingResponse
                {
                    BookingId = booking!.BookingId,
                    CheckIn = booking.CheckIn,
                    CheckOut = booking.CheckOut,
                    GuestQuant = booking.GuestQuant,
                    Room = new RoomDto
                    {
                        RoomId = room!.RoomId,
                        Name = room.Name,
                        Capacity = room.Capacity,
                        Image = room.Image,
                        Hotel = new HotelDto
                        {
                            HotelId = hotel!.HotelId,
                            Name = hotel!.Name,
                            Address = hotel!.Address,
                            CityId = city!.CityId,
                            CityName = city!.Name,
                            State = city!.State
                        }
                    }
                }).FirstOrDefault();

            return bookingResponse!;
        }

        public Room GetRoomById(int RoomId)
        {
            throw new NotImplementedException();
        }

    }

}

public class GuestQuantityExceedsCapacityException : Exception { }
public class RoomNotFoundException : Exception { }
