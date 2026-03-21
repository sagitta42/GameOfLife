using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;

namespace Utils
{
    public class Database
    {
        string name = "game_of_life";
        SqlConnection conn;

        public Database()
        {
            string connectionString = $"Server=localhost;Database={name};Trusted_Connection=true;TrustServerCertificate=true;";
            conn = new SqlConnection(connectionString);
            conn.Open();
        }


        public int GetPatternId(string name)
        {
            string query = $"select id from dbo.pattern_templates where dbo.pattern_templates.name = '{name}'";
            var command = new SqlCommand(query, conn);
            var reader = command.ExecuteReader();

            // TODO: pattern name not found
            int id = 0;
            while (reader.Read())
            {
                id = (int)reader["id"];
            }
            return id;
        }
    }
}
