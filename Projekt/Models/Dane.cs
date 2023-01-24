using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class Dane
    {
        public int Id { get; set; }

        [Display(Name = "Temat")]
        public string? Topic { get; set; }
        [Display(Name = "Treść")]
        public string? Content { get; set; }

        public string? UserId { get; set; }
        public virtual IdentityUser? User { get; set; }
    }
}
