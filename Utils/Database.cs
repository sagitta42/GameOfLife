using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;

namespace Utils
{
    public class Database : IDisposable
    {
        string name = "game_of_life";
        SqlConnection conn;

        public Database()
        {
            string connectionString = $"Server=localhost;Database={name};Trusted_Connection=true;TrustServerCertificate=true;";
            conn = new SqlConnection(connectionString);
            conn.Open();
        }

        ~Database() {conn.Close();}

        public void Dispose() { conn.Close(); }

        public int[][] GetPatternCoordinates(string pattern)
        {
            int id = GetPatternIdFromName(pattern);
            int[][] ret = GetPatternCoordinatesFromId(id);
            return ret;
        }

        private int[][] GetPatternCoordinatesFromId(int id)
        {
            string query = $"select x,y from dbo.pattern_coordinates where dbo.pattern_coordinates.id = {id}";
            using SqlDataReader reader = GetReader(query);

            List<int> x = new List<int>();
            List<int> y = new List<int>();
            while (reader.Read())
            {
                x.Add((int)reader["x"]);
                y.Add((int)reader["y"]);
            }

            int[][] ret;
            ret = new int[2][];
            ret[0] = x.ToArray();
            ret[1] = y.ToArray();
            return ret;
        }

        private int GetPatternIdFromName(string name)
        {
            string query = $"select id from dbo.pattern_templates where dbo.pattern_templates.name = '{name}'";
            using SqlDataReader reader = GetReader(query);

            // TODO: pattern name not found
            int id = 0;
            while (reader.Read())
            {
                id = (int)reader["id"];
            }
            return id;
        }

        private SqlDataReader GetReader(string query)
        {
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
    }
}
