using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for UsersService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[ScriptService]
public class UsersService : System.Web.Services.WebService
{
    #region select functions

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<UsersClass> GetAll()
    {
        string sql_str = "SELECT * FROM [Users]";
        DataTable all = Dbase.SelectFromTable(sql_str);
        int count = all.Rows.Count;

        List<UsersClass> result = new List<UsersClass>();

        for(int i = 0; i < count; i++)
        {
            result.Add(new UsersClass(all.Rows[i]));
        }

        return result;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public UsersClass GetByID(int id)
    {
        string sql_str = "SELECT * FROM [Users] WHERE [ID]=" + id.ToString();
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        if (user_dt.Rows.Count == 0) return null;

        UsersClass user = new UsersClass(user_dt.Rows[0]);
        return user;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public UsersClass GetByCredentials(string username, string password)
    {
        string sql_str = "SELECT * FROM [Users] WHERE [Username]='{0}' AND [Password]='{1}'";
        string.Format(sql_str, username, password);
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        if (user_dt.Rows.Count == 0) return null;

        UsersClass user = new UsersClass(user_dt.Rows[0]);
        return user;
    }

    #endregion

    #region utility functions

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public bool UserNameTaken(string username)
    {
        string sql_str = "SELECT * FROM [Users] WHERE [Username]='{0}'";
        string.Format(sql_str, username);
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        return user_dt.Rows.Count > 0;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public bool UserExists(string username, string password)
    {
        UsersClass user = GetByCredentials(username, password);
        return user != null;

    }
    #endregion

}
