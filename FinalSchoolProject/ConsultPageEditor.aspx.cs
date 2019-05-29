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

            FillConsultPagesDDL();
        }
    }

    private void FillUsersGV()
    {

    }

    private void FillConsultPagesDDL()
    {

        DataTable consult_pages = ConsultPagesClass.GetAll();

        foreach (DataRow consult_page in consult_pages.Rows)
        {
            ConsultPagesClass cp = ConsultPagesClass.FromDataRow(consult_page);
            ListItem li = new ListItem();
            li.Value = cp.ID.ToString();
            li.Text = cp.PageName;
            ConsultPagesDDL.Items.Add(li);
        }

    }

    protected void ConsultPagesDDL_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}