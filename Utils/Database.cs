using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace Utils
{
    public class DBSettings
    {
        public string mode { get; set; }
        public string db_name { get; set; }
    }
    public class SQLServerConfig
    {
        public string username { get; set; }
        public string password { get; set; }
        public int port { get; set; }
    }

    public class Database : IDisposable
    {
        IDbConnection conn;
        // TODO: improve
        string tbl_pre;

        public Database()
        {
            DBSettings? dbSettings = JsonSerializer.Deserialize<DBSettings>(
                File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "db", "db_settings.json"))
            );
            if (dbSettings == null) { throw new Exception("DB mode config not found!"); }

            string connectionString;
            if (dbSettings.mode == "sqlserver")
            {
                SQLServerConfig? configSqlServer = JsonSerializer.Deserialize<SQLServerConfig>(
                    File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "db", "sql_server_config.json"))
                );
                if (configSqlServer == null) { throw new Exception("SQL Server config not found!"); }
                connectionString = $"Server=localhost,{configSqlServer.port};Database={dbSettings.db_name};User Id={configSqlServer.username};Password={configSqlServer.password};TrustServerCertificate=true;";
                conn = new SqlConnection(connectionString);
                tbl_pre = "dbo.";
            }
            else if(dbSettings.mode == "sqlite")
            {
                string dbPath = Path.Combine(AppContext.BaseDirectory, "db", $"{dbSettings.db_name}.db");
                connectionString = $"Data Source={dbPath}";
                conn = new SqliteConnection(connectionString);
                tbl_pre = "";
            }
            else
            {
                throw new Exception($"{dbSettings.mode} not supported; use 'sqlserver' or 'sqlite'");
            }

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
            string query = $"select x,y from {tbl_pre}pattern_coordinates where {tbl_pre}pattern_coordinates.id = {id}";
            using IDataReader reader = GetReader(query);

            List<int> x = new List<int>();
            List<int> y = new List<int>();
            while (reader.Read())
            {
                x.Add(Convert.ToInt32(reader["x"]));
                y.Add(Convert.ToInt32(reader["y"]));
            }

            int[][] ret;
            ret = new int[2][];
            ret[0] = x.ToArray();
            ret[1] = y.ToArray();
            return ret;
        }

        private int GetPatternIdFromName(string name)
        {
            string query = $"select id from {tbl_pre}pattern_templates where {tbl_pre}pattern_templates.name = '{name}'";
            using IDataReader reader = GetReader(query);

            // TODO: pattern name not found
            int id = 0;
            while (reader.Read())
            {
                id = Convert.ToInt32(reader["id"]);
            }
            return id;
        }

        private IDataReader GetReader(string query)
        {
            IDbCommand command = conn.CreateCommand();
            command.CommandText = query;
            IDataReader reader = command.ExecuteReader();
            return reader;
        }
    }
}
