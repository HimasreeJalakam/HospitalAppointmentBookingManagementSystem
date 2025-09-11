using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Models.Data;

public class DB
{
        private SqlConnection connection;

        public DB(AppDbContext context)
        {
            connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
        }


        #region Connection
        public SqlConnection GetConnection()
        {
            connection.Open();
            return connection;
        }
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

        }
        #endregion

        #region Private Methods 
        private SqlParameter createSqlParameter(string name, object value)
        {
            SqlParameter objSqlParameter = new SqlParameter(name, value);
            return objSqlParameter;
        }
        public DataSet ConvertDataReaderToDataSet(SqlDataReader reader)
        {
            DataSet dataSet = new DataSet();
            do
            {
                // Create new data table

                DataTable schemaTable = reader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    // A query returning records was executed

                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        // Create a column name that is unique in the data table
                        string columnName = (string)dataRow["ColumnName"]; //+ "<C" + i + "/>";
                                                                           // Add the column definition to the data table
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }

                    dataSet.Tables.Add(dataTable);

                    // Fill the data table we just created

                    while (reader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < reader.FieldCount; i++)
                            dataRow[i] = reader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    // No records were returned

                    DataColumn column = new DataColumn("RowsAffected");
                    dataTable.Columns.Add(column);
                    dataSet.Tables.Add(dataTable);
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = reader.RecordsAffected;
                    dataTable.Rows.Add(dataRow);
                }
            }
            while (reader.NextResult());
            return dataSet;
        }
        #endregion

        #region SQL Specific
        public int InsertUpdateOrDelete(string QueryOrSPName, nameValuePairList NameValuePairObject, bool IsSP = false)
        {
            SqlCommand cmdObject = new SqlCommand(QueryOrSPName, GetConnection());

            if (IsSP)
            {
                cmdObject.CommandType = CommandType.StoredProcedure;
            }

            foreach (nameValuePair objList in NameValuePairObject)
            {
                cmdObject.Parameters.Add(createSqlParameter(objList.ColName, objList.Value));
            }

            int status = 0;

            try
            {
                status = cmdObject.ExecuteNonQuery();

                return status;
            }
            catch (Exception exp)
            {
                return status;
            }
            finally
            {
                CloseConnection();
            }


        }
        public int InsertUpdateOrDelete(string InsertQuery)
        {
            SqlCommand cmdObject = new SqlCommand(InsertQuery, GetConnection());

            int status = 0;

            try
            {
                status = cmdObject.ExecuteNonQuery();
                if (connection.State == ConnectionState.Open)
                {
                    CloseConnection();
                }
                return status;
            }
            catch (Exception exp)
            {
                return status;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    CloseConnection();
                }
            }


        }
        public DataTable FillAndReturnDataTable(string SelectQuery)
        {

            SqlDataAdapter adp = new SqlDataAdapter(SelectQuery, GetConnection());
            DataTable dt = new DataTable();
            adp.Fill(dt);
            CloseConnection();
            return dt;

        }
        public DataTable FillAndReturnDataSet(string Query, nameValuePairList NameValuePairObject)
        {
            try
            {
                SqlCommand CommandObject = new SqlCommand(Query, GetConnection());

                //CommandObject.CommandType = CommandType.StoredProcedure;

                foreach (nameValuePair objList in NameValuePairObject)
                {
                    CommandObject.Parameters.Add(createSqlParameter(objList.ColName, objList.Value));
                }

                SqlDataReader ReaderObject = CommandObject.ExecuteReader();

                DataSet ds = ConvertDataReaderToDataSet(ReaderObject);

                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        #region Name Value Pair and List...
        //Dictionary<string, object>
        public class nameValuePairList : List<nameValuePair>
        {

        }


        public class nameValuePair //: Dictionary<string, object> 
        {
            public string ColName { get; set; }
            public object Value { get; set; }
            public nameValuePair(string Name, object ColValue)
            {
                this.ColName = Name;
                this.Value = ColValue;
            }
        }
        #endregion
        #endregion
}
