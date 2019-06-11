using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class PageMembersEditor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WarningLabel.Text = "";
        WarningLabel.Visible = false;

        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Response.Redirect("All.aspx");
        }
        UsersClass current_user = UsersClass.GetByID(Convert.ToInt32(Session["CurrentUserID"]));
        if (!current_user.IsModerator())
        {
            Response.Redirect("All.aspx");
        }

        FillConsultPagesDDL();

        object pageid = Request.Form["PageID"];

        if(pageid != null && pageid.ToString() != "")
        {
            Session["PageID"] = pageid;
        }

        if ((pageid == null || pageid.ToString() == "") && (Session["PageID"] == null || Session["PageID"].ToString() == ""))
        {
            pageid = ConsultPagesDDL.Items[0].Value;
            Session["PageID"] = pageid;
        }
        else if (Session["PageID"] != null && Session["PageID"].ToString() != "")
        {
            pageid = Session["PageID"];
            ConsultPagesDDL.SelectedValue = pageid.ToString();
        }

        FillUsersInPageGV(Convert.ToInt32(pageid));
        FillUsersNotInPageGV(Convert.ToInt32(pageid));

    }

    private void FillUsersInPageGV(int PageID)
    {

        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);

        if (page == null)
        {
            WarningLabel.Text = "invalid page";
            WarningLabel.Visible = true;
            return;
        }

        DataTable users_dt = page.GetUsersInPage();
        UsersInPageGV.DataSource = users_dt;
        UsersInPageGV.DataBind();

    }

    private void FillUsersNotInPageGV(int PageID)
    {

        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);

        if (page == null)
        {
            WarningLabel.Text = "invalid page";
            WarningLabel.Visible = true;
            return;
        }

        DataTable users_dt = page.GetUsersNotInPage();
        UsersNotInPageGV.DataSource = users_dt;
        UsersNotInPageGV.DataBind();

    }

    private void FillConsultPagesDDL()
    {
        ConsultPagesDDL.Items.Clear();

        UsersClass current_user = UsersClass.GetByID(Convert.ToInt32(Session["CurrentUserID"]));

        DataTable consult_pages = current_user.GetMyModeratedPages();

        foreach (DataRow consult_page in consult_pages.Rows)
        {
            ConsultPagesClass cp = ConsultPagesClass.FromDataRow(consult_page);
            ListItem li = new ListItem();
            li.Value = cp.ID.ToString();
            li.Text = cp.PageName;
            ConsultPagesDDL.Items.Add(li);
        }

    }
}