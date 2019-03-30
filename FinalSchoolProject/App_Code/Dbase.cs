using System;
using System.Data;
using System.Configuration; 

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.OleDb;

/// <summary>
/// Summary description for Dbase
/// </summary>
public class Dbase
{
    public Dbase()
    {

    }

    public static OleDbConnection MakeConnection(string dbFile = "DB.accdb")
    {
        OleDbConnection c = new OleDbConnection();
        if (dbFile.Contains(".accdb"))
            // MS Access >=2007
            c.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + dbFile);
        else
            // MS Access 2003
            c.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + dbFile);
        c.Open();
        return c;
    }

    public static DataTable SelectFromTable(string strSql, string FileName = "DB.accdb")
    {
        OleDbConnection c = MakeConnection(FileName);
        OleDbCommand comm = new OleDbCommand();
        comm.CommandText = strSql;
        comm.Connection = c;
        DataTable dt = new DataTable();
        OleDbDataAdapter da = new OleDbDataAdapter(comm);
        da.Fill(dt);
        c.Close();
        return dt;
    }

    public static void ChangeTable(string strSql, string FileName = "DB.accdb")
    {
        OleDbConnection c = MakeConnection(FileName);
        OleDbCommand comm = new OleDbCommand();
        comm.CommandText = strSql;
        comm.Connection = c;
        comm.ExecuteNonQuery();
        c.Close();

    }
}