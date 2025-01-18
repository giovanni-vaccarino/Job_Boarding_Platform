using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using backend.Shared.Enums;

namespace backend.Data.Entities;

public class PlatformFeedback : EntityBase
{
   [Required]
   public required string Text { get; set; }
   
   [Required]
   public required Rating Rating { get; set; }
   
   public User User { get; set; }
   
   [ForeignKey("UserId")]
   public required int UserId { get; set; }
}