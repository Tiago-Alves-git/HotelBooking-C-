namespace TrybeHotel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 1. Implemente as models da aplicação
public class Booking
{
  public int BookingId { get; set; } // Chave primária
  public int UserId { get; set; } // Chave estrangeira para User
  public int RoomId { get; set; } // Chave estrangeira para Room
  public DateTime CheckIn { get; set; }
  public DateTime CheckOut { get; set; }
  public int GuestQuant { get; set; }

  // Propriedades de navegação
  public User? User { get; set; } // Relacionamento com User
  public Room? Room { get; set; } // Relacionamento com Room
}