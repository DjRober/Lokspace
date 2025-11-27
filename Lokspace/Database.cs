using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

// Clase database para conexiones a la base de datos

namespace Lokspace
{
    internal class Database
    {
        private string connectionString = "server=localhost;database=Lokspace;uid=root;pwd=;";
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
