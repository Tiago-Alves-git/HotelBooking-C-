namespace TrybeHotel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 1. Implemente as models da aplicação
public class User
{
  public int UserId { get; set; } // Chave primária
  public string? Name { get; set; }
  public string? Email { get; set; }
  public string? Password { get; set; }
  public string? UserType { get; set; }

  // Propriedade de navegação para Bookings (um usuário pode ter várias reservas)
  public ICollection<Booking>? Bookings { get; set; }
}