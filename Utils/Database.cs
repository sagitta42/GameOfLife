using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;

namespace Utils
{
    public class DBConfig
    {
        public string database { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int port { get; set; }
    }

    public class Database : IDisposable
    {
        SqlConnection conn;

        public Database()
        {
            DBConfig? config = JsonSerializer.Deserialize<DBConfig>(
                File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "db_config.json"))
            );
            if(config == null) {throw new Exception("DB config not found!"); }
            string connectionString = $"Server=localhost,{config.port};Database={config.database};User Id={config.username};Password={config.password};TrustServerCertificate=true;";
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
