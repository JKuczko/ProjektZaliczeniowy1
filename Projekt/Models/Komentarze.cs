using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Projekt.Models
{
    public class Komentarze
    { 
            public int Id { get; set; }
            [Display(Name = "Treść")]
            public string? Comment{ get; set; }
            public int? PostId { get; set; }
            public virtual Dane? Dane { get; set; }
            public string? UserId { get; set; }
            public virtual IdentityUser? User { get; set; }

    }
}
