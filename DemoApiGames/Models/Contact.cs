namespace DemoApiGames.Models
{
    /// <summary>
    /// Classe réprésentant un contact de la base de données
    /// </summary>
    public class Contact
    {
        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
    }
}
