using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace TRD2022.Hangfire.Infra.Factories
{
    public class ConnectionFactory
    {
        private MySqlConnection Connection;
        private string connString = "";

        public ConnectionFactory(string connectionString)
        {
            connString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            if (Connection == null)
                Connection = new MySqlConnection(connString);

            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();

            return Connection;
        }

        public MySqlCommand GetCommand()
        {
            return GetConnection().CreateCommand();
        }

        // TODO: camada que chama esse método precisa utilizar try catch
        public DbDataReader GetReader(string cmdText, CommandType cmdType = CommandType.Text, Dictionary<string, object> param = null)
        {
            using (var cmd = this.GetCommand())
            {
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (param != null)
                {
                    foreach (var pr in param)
                    {
                        var parameter = cmd.CreateParameter();
                        parameter.ParameterName = pr.Key;
                        parameter.Value = pr.Value;
                        if (pr.Value != null && pr.Value.GetType().Name == "Boolean")
                            parameter.MySqlDbType = MySqlDbType.Bit;
                        cmd.Parameters.Add(parameter);
                    }
                }

                return cmd.ExecuteReader();
            }
        }

        // TODO: camada que chama esse método precisa utilizar try catch
        public bool ExecuteNonQuery(string cmdText, CommandType cmdType = CommandType.Text, Dictionary<string, object> param = null)
        {
            using (var cmd = this.GetCommand())
            {
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (param != null)
                {
                    foreach (var pr in param)
                    {
                        var parameter = cmd.CreateParameter();
                        parameter.ParameterName = pr.Key;
                        parameter.Value = pr.Value;
                        if (pr.Value != null && pr.Value.GetType().Name == "Boolean")
                            parameter.MySqlDbType = MySqlDbType.Bit;
                        cmd.Parameters.Add(parameter);
                    }
                }

                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public object ExecuteScalar(string cmdText, CommandType cmdType = CommandType.Text, Dictionary<string, object> param = null)
        {
            using (var cmd = this.GetCommand())
            {
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (param != null)
                {
                    foreach (var pr in param)
                    {
                        var parameter = cmd.CreateParameter();
                        parameter.ParameterName = pr.Key;
                        parameter.Value = pr.Value;
                        if (pr.Value != null && pr.Value.GetType().Name == "Boolean")
                            parameter.MySqlDbType = MySqlDbType.Bit;
                        cmd.Parameters.Add(parameter);
                    }
                }

                return cmd.ExecuteScalar();
            }
        }

        public void Dispose()
        {
            if (Connection != null && Connection.State == System.Data.ConnectionState.Open)
                Connection.Close();
            Connection.Dispose();
        }
    }
}
