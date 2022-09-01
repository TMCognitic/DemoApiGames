using System.ComponentModel.DataAnnotations;

namespace DemoApiGames.Models.Forms
{
    public class EditContactForm
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        public string LastName { get; set; }
        [Required]
        [MinLength(1)]
        public string FirstName { get; set; }
    }
}