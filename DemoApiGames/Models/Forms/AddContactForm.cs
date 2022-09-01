using System.ComponentModel.DataAnnotations;

namespace DemoApiGames.Models.Forms
{
    /// <summary>
    /// Classe reprenant les informations nécessaires pour l'ajout d'un contact
    /// </summary>
    public class AddContactForm
    {
        /// <summary>
        /// Les attributs Required et MinLength sont là pour valider
        /// le formulaire c'est ce qu'on appelle des "ValidationAttributes"
        /// </summary>
        [Required]
        [MinLength(1)]
        public string LastName { get; set; }
        [Required]
        [MinLength(1)]
        public string FirstName { get; set; }
    }
}
