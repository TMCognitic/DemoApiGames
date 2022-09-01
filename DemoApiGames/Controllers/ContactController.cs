using CogniticTools.Connections.Database;
using DemoApiGames.Models;
using DemoApiGames.Models.Forms;
using DemoApiGames.Models.Mappers;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DemoApiGames.Controllers
{
    /// <summary>
    /// Tout controller prend un suffixe Controller et doit hériter de ControllerBase
    /// Afin de le différencier des Controllers utilisé dans les sites MVC (Model View Controller)
    /// celui-ci l'attribut [ApiController] et on spécifie la structure de l'url à utiliser
    /// au travers de l'attribut [Route()] dans ce cas https://localhost:7022/api/contact
    /// Le port 7022 est défini dans le fichier launchSettings.json en ligne 9
    /// 
    /// Le script de création de la base de données et de la table, se trouve dans le répertoire ScriptDB.sql
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IDbConnection _connection;

        public ContactController(IDbConnection connection)
        {
            _connection = connection;
        }

        // GET: api/<ContactController>
        /// <summary>
        /// Méthode retournant la liste des contacts
        /// </summary>
        /// <returns>Liste des contacts</returns>
        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            return _connection.ExecuteReader("SELECT Id, LastName, FirstName FROM Contact;", (dr) => dr.ToContact(), true);
        }

        // GET api/<ContactController>/5
        /// <summary>
        /// Retourne un contact sur base de son Id
        /// </summary>
        /// <param name="id">id du contact souhaité</param>
        /// <returns>Retourne le contact s'il existe, sinon null</returns>
        [HttpGet("{id}")]
        public Contact? Get(int id)
        {
            return _connection.ExecuteReader("SELECT Id, LastName, FirstName FROM Contact WHERE Id = @Id;", (dr) => dr.ToContact(), true, new { Id = id }).SingleOrDefault();
        }

        // POST api/<ContactController>
        /// <summary>
        /// Ajoute un contact
        /// </summary>
        /// <param name="form">données du contact à ajouter</param>
        [HttpPost]
        public void Post([FromBody] AddContactForm form)
        {
            _connection.ExecuteNonQuery("INSERT INTO Contact (LastName, FirstName) VALUES (@LastName, @FirstName);", new { form.LastName, form.FirstName });
        }

        // PUT api/<ContactController>/5
        /// <summary>
        /// Modifie un contact sur base de l'id Reçu
        /// </summary>
        /// <param name="id">id du contact à supprimer</param>
        /// <param name="form">Contient les valeurs à modifier</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] EditContactForm form)
        {
            _connection.ExecuteNonQuery("UPDATE Contact SET LastName = @LastName, FirstName = @FirstName WHERE Id = @Id;", new { form.Id, form.LastName, form.FirstName });
        }

        // DELETE api/<ContactController>/5
        /// <summary>
        /// Supprime un contact
        /// </summary>
        /// <param name="id">Id du contact à supprimer</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _connection.ExecuteNonQuery("DELETE FROM Contact WHERE Id = @Id;", new { Id = id });
        }
    }
}
