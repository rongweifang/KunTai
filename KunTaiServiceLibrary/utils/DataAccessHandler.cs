using System;
using System.Data;
using System.Data.SqlClient;
using Warrior.DataAccessUtils.Desktop;

namespace KunTaiServiceLibrary
{
    /// <summary>
    /// 数据库处理类
    /// </summary>
    public class DataAccessHandler
    {
        public DataAccessHandler()
        {
        }

        /// <summary>
        /// 获取数据库连接状态
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>返回数据库的当前状态</returns>
        public string getDBConnectionState(string connectionString)
        {
            string _result = string.Empty;

            SqlConnection _conn = null;
            try
            {
                _conn = new SqlConnection(connectionString == string.Empty ? Config.DBConnectionString : connectionString);
                _conn.Open();

                _result = "DATABASE CONNECTION IS SUCCESSFUL.";
            }
            catch (Exception ex)
            {
                _result = string.Format("CONNECTION STRING : {0}\r\n{1}", _conn.ConnectionString, ex.Message);
            }
            finally
            {
                _conn.Close();
            }



            return _result;
        }

        public DataSet executeDatasetResult(string commandText, SqlParameter[] parameter)
        {
            return executeDatasetResult(Config.DBConnectionString, commandText, parameter);
        }

        public DataSet executeDatasetResult(string connectionString, string commandText, SqlParameter[] parameter)
        {
            DataSet _dataSet = null;
            try
            {
                using (SqlConnection _conn = new SqlConnection(connectionString))
                {
                    _conn.Open();
                    _dataSet = SqlServer.ExecuteDataset(_conn, CommandType.Text, commandText, parameter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _dataSet;
        }

        public DataSet executeEmptyDataset()
        {
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(new DataTable());

            return dataSet;
        }

        public string executeNonQueryResult(string connectionString, string commandText, SqlParameter[] parameter)
        {
            string _result = string.Empty;
            try
            {
                using (SqlConnection _conn = new SqlConnection(connectionString))
                {
                    _conn.Open();
                    using (SqlTransaction _transaction = _conn.BeginTransaction())
                    {
                        try
                        {
                            SqlServer.ExecuteNonQuery(_transaction, CommandType.Text, commandText, parameter);
                            _transaction.Commit();

                            _result = Result.getResultXml(string.Empty);
                        }
                        catch (Exception ex)
                        {
                            _transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _result;
        }

        public string executeNonQueryResult(string commandText, SqlParameter[] parameter)
        {
            return executeNonQueryResult(Config.DBConnectionString, commandText, parameter);
        }

        public string executeScalarResult(string connectionString, string commandText, SqlParameter[] parameter)
        {
            string _result = string.Empty;
            try
            {
                using (SqlConnection _conn = new SqlConnection(connectionString))
                {
                    _conn.Open();

                    try
                    {
                        object _object = SqlServer.ExecuteScalar(_conn, CommandType.Text, commandText, parameter);

                        if (_object != null)
                            _result = _object.ToString();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _result;
        }

        public string executeScalarResult(string commandText, SqlParameter[] parameter)
        {
            return executeScalarResult(Config.DBConnectionString, commandText, parameter);
        }

    }
}
