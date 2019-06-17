using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ConsultPageEditor : System.Web.UI.Page
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

        DataTable pages_dt;

        if (ViewState["UsersGVDataSource"] != null)
        {
            pages_dt = (DataTable)ViewState["UsersGVDataSource"];
        }
        else
        {
            pages_dt = ConsultPagesClass.GetAll();
            ViewState["UsersGVDataSource"] = pages_dt;
        }

        ConsultPagesGV.DataSource = pages_dt;
        ConsultPagesGV.DataBind();
        ConsultPagesGV.HeaderRow.TableSection = TableRowSection.TableHeader;


    }
}