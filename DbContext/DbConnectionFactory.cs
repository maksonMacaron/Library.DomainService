using MySql.Data.MySqlClient;

namespace Library.DomainService.DbContext
{
    public static class DbConnectionFactory
    {
        private const string ConnectionString =
            "Server=localhost;Port=6033;Database=library_db;Uid=root;Pwd=rootpass;Charset=utf8mb4;";

        public static MySqlConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}