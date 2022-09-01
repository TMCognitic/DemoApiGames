using System.Data;

namespace DemoApiGames.Models.Mappers
{
    public static class DataRecordExtensions
    {
        public static Contact ToContact(this IDataRecord record)
        {
            return new Contact()
            {
                Id = (int)record["Id"],
                LastName = (string)record["LastName"],
                FirstName = (string)record["FirstName"],
            };
        }
    }
}
