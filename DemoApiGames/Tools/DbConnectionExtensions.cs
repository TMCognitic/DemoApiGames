using System.Data;
using System.Data.Common;
using System.Reflection;

namespace CogniticTools.Connections.Database
{
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Méthode d'extension permettant l'exécute d'un ordre DML (Insert, Update, Delete) au niveau de la db
        /// </summary>
        /// <param name="dbConnection">L'instance de connexion à la db</param>
        /// <param name="query">La requête a exécuter</param>
        /// <param name="parameters">Les paramètres sous forme d'objet</param>
        /// <returns>Le nombre de lignes affectées</returns>
        public static int ExecuteNonQuery(this IDbConnection dbConnection, string query, object? parameters = null)
        {            
            using (IDbCommand dbCommand = CreateCommand(dbConnection, query, parameters))
            {
                if (dbConnection.State is ConnectionState.Closed)
                    dbConnection.Open();

                return dbCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Méthode d'extension permettant la sélection d'un valeur unique en base de données
        /// </summary>
        /// <param name="dbConnection">L'instance de connexion à la db</param>
        /// <param name="query">La requête a exécuter</param>
        /// <param name="parameters">Les paramètres sous forme d'objet</param>
        /// <returns>La valeur unique ou null</returns>
        public static object? ExecuteScalar(this IDbConnection dbConnection, string query, object? parameters = null)
        {
            using (IDbCommand dbCommand = CreateCommand(dbConnection, query, parameters))
            {
                if (dbConnection.State is ConnectionState.Closed)
                    dbConnection.Open();

                object? result = dbCommand.ExecuteScalar();
                return result is DBNull ? null : result;
            }
        }

        public static IEnumerable<TResult> ExecuteReader<TResult>(this IDbConnection dbConnection, string query, Func<IDataRecord, TResult> selector, bool immediately, object? parameters = null)
        {
            IEnumerable<TResult> result = dbConnection.ExecuteReader(query, selector, parameters);
            return (!immediately) ? result : result.ToList();
        }

        /// <summary>
        /// Méthode d'extension permettant de récupérer un résultat tabulaire et de le transformer en une séquence d'objets
        /// </summary>
        /// <typeparam name="TResult">Le type d'ojet généré par le paramètre 'séléctor'</typeparam>
        /// <param name="dbConnection">L'instance de connexion à la db</param>
        /// <param name="query">La requête a exécuter</param>
        /// <param name="selector">La méthode permettant la trnasformation du DataRecord en objet</param>
        /// <param name="parameters">Les paramètres sous forme d'objet</param>
        /// <returns></returns>
        public static IEnumerable<TResult> ExecuteReader<TResult>(this IDbConnection dbConnection, string query, Func<IDataRecord, TResult> selector, object? parameters = null)
        {
            ArgumentNullException.ThrowIfNull(selector);

            using (IDbCommand dbCommand = CreateCommand(dbConnection, query, parameters))
            {
                if (dbConnection.State is ConnectionState.Closed)
                    dbConnection.Open();

                using (IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        yield return selector(dataReader);
                    }
                }
            }
        }

        /// <summary>
        /// Crée une commande sur base des valeur reçue en paramètre
        /// </summary>
        /// <param name="dbConnection">L'instance de connexion à la db</param>
        /// <param name="query">La requête a exécuter</param>
        /// <param name="parameters">Les paramètres sous forme d'objet</param>
        /// <returns>La commande de type IDbCommand exécutatble sur le serveur</returns>
        private static IDbCommand CreateCommand(IDbConnection dbConnection, string query, object? parameters)
        {
            ArgumentNullException.ThrowIfNull(query);

            //Instancie la commande
            IDbCommand dbCommand = dbConnection.CreateCommand();
            //Défini la requête à exécuter
            dbCommand.CommandText = query;

            //Défini par réflexion les paramètres de la requête en fonction des propriétés contenues
            //dans la variable parameters
            if (parameters is not null)
            {
                Type type = parameters.GetType();
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    IDataParameter dbParameter = dbCommand.CreateParameter();
                    dbParameter.ParameterName = propertyInfo.Name;
                    dbParameter.Value = propertyInfo.GetValue(parameters) ?? DBNull.Value;
                    dbCommand.Parameters.Add(dbParameter);
                }
            }

            return dbCommand;
        }
    }
}
