using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class UserEditor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Logged"] == null || (bool)Session["Logged"] == false || Session["CurrentUserIsAdmin"] == null || (bool)Session["CurrentUserIsAdmin"] == false)
            {
                Response.Redirect("All.aspx");
            }

            FillUsersGV();

        }
    }

    private void FillUsersGV()
    {

        DataTable dt = UsersClass.GetAll();
        ViewState["UsersGVDataSource"] = dt;
        UsersGV.DataSource = dt;
        UsersGV.DataBind();

    }


    protected void UsersGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        UsersGV.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)ViewState["UsersGVDataSource"];
        UsersGV.DataSource = dt;
        UsersGV.DataBind();
    }
}