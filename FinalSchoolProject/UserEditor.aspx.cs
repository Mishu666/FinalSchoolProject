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


            string users_page_no = Request.Form["UsersGV_PageNo"];

            if (users_page_no != null && users_page_no != "")
            {
                ViewState["UsersGV_PageNo"] = users_page_no;
            }

            if ((users_page_no == null || users_page_no == "") && (ViewState["UsersGV_PageNo"] == null || ViewState["UsersGV_PageNo"].ToString() == ""))
            {
                users_page_no = "1";
                ViewState["UsersGV_PageNo"] = users_page_no;
            }
            else if (ViewState["UsersGV_PageNo"] != null && ViewState["UsersGV_PageNo"].ToString() != "")
            {
                users_page_no = ViewState["UsersGV_PageNo"].ToString();
            }

            FillUsersGV(int.Parse(users_page_no));
        }
    }

    private void FillUsersGV(int PageNo)
    {

        DataTable users_dt;

        if (ViewState["UsersGVDataSource"] != null)
        {
            users_dt = (DataTable)ViewState["UsersGVDataSource"];
        }
        else
        {
            users_dt = UsersClass.GetAllExcept(Convert.ToInt32(Session["CurrentUserID"]));
            ViewState["UsersGVDataSource"] = users_dt;
        }

        UsersGV.DataSource = users_dt;
        UsersGV.PageIndex = PageNo;
        UsersGV.DataBind();

    }

}