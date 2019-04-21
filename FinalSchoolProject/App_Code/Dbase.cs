using System;
using System.Data;
using System.Web;
using System.Data.SQLite;

public class Dbase
{
    public Dbase()
    {

    }

    public static SQLiteConnection MakeConnection(string dbFile = "SQLiteDB.db")
    {
        SQLiteConnection c = new SQLiteConnection();
        c.ConnectionString = "Data Source=" +
            HttpContext.Current.Server.MapPath("~/App_Data/" + dbFile) +
            ";Version=3;";
             c.Open();
        return c;
    }

    public static DataTable SelectFromTable(string strSQLite, string FileName = "SQLiteDB.db")
    {
        SQLiteConnection c = MakeConnection(FileName);
        SQLiteCommand comm = new SQLiteCommand();
        comm.CommandText = strSQLite;
        comm.Connection = c;
        DataTable dt = new DataTable();
        SQLiteDataAdapter da = new SQLiteDataAdapter(comm);
        da.Fill(dt);
        c.Close();
        return dt;
    }

    public static void ChangeTable(string strSQLite, string FileName = "SQLiteDB.db")
    {
        SQLiteConnection c = MakeConnection(FileName);
        SQLiteCommand comm = new SQLiteCommand();
        comm.CommandText = strSQLite;
        comm.Connection = c;
        comm.ExecuteNonQuery();
        c.Close();

    }
}