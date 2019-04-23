using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for MessagesClass
/// </summary>
public class MessagesClass
{
    public int ID { get; private set; }
    public int SenderID, RecipientID;
    public string Title, Body;
    public DateTime SendDate;

    #region constructors

    private MessagesClass()
    {

    }

    private MessagesClass(int ID, int SenderID,
        int RecipientID, string Body, string Title, DateTime SendDate)
    {
        this.ID = ID;
        this.SenderID = SenderID;
        this.RecipientID = RecipientID;
        this.Body = Body;
        this.Title = Title;
        this.SendDate = SendDate;
    }

    public static MessagesClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        MessagesClass obj = new MessagesClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            SenderID = Convert.ToInt32(dr["SenderID"]),
            RecipientID = Convert.ToInt32(dr["RecipientID"]),
            Body = dr["Body"].ToString(),
            Title = dr["Title"].ToString(),
            SendDate = Convert.ToDateTime(dr["SendDate"])
        };
        return obj;
    }

    public static MessagesClass CreateNew(int SenderID, int RecipientID, string Title, string Body)
    {
        MessagesClass message = new MessagesClass
        {
            SenderID = SenderID,
            RecipientID = RecipientID,
            Body = Body,
            Title = Title,
            SendDate = DateTime.Now
        };
        message.Insert();
        return message;
    }

    #endregion

    #region sql functions

    private void Insert()
    {
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }

        string sql_str = "INSERT INTO [Messages] " +
            "([SenderID], [RecipientID], [Title], [Body], [SendDate]) " +
            "VALUES ({0}, {1}, '{2}' , '{3}', #{4}#) ";

        sql_str = string.Format(sql_str, this.SenderID, this.RecipientID, this.Title, this.Body, this.SendDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [Messages] " +
            "SET [SenderID] = {0}, [RecipientID] = {1}, " +
            "[Title] = '{2}',[Body] = '{3}', [SendDate] = #{4}#";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.SenderID, this.RecipientID, this.Title, this.Body, this.SendDate);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [Messages] WHERE [ID]=" + this.ID;
        Dbase.ChangeTable(sql_str);
    }

    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [Messages]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static MessagesClass GetByID(int ID)
    {
        KeyValuePair<string, object> id_pair = new KeyValuePair<string, object>("ID", ID);
        DataTable obj = GetByProperties(id_pair);
        if (obj == null || obj.Rows.Count == 0) return null;
        else return FromDataRow(obj.Rows[0]);
    }
    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [Messages]";
        string prepend, append;

        if (pairs.Length > 0) sql_str += " WHERE ";

        for (int i = 0; i < pairs.Length; i++)
        {
            prepend = "";
            append = "";
            if (pairs[i].Value is string)
            {
                prepend = "'";
                append = "'";
            }
            if (pairs[i].Value is DateTime)
            {
                prepend = "#";
                append = "#";
            }
            sql_str += "[{0}] = {1}{2}{3}";
            if (i < pairs.Length - 1) sql_str += " AND ";

            sql_str = string.Format(sql_str, pairs[i].Key, prepend, pairs[i].Value, append);

        }
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        return user_dt;
    }

    #endregion
}