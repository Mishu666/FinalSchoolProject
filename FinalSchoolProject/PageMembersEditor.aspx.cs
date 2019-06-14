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
        if (!IsPostBack)
        {
            WarningLabel.Text = "";
            WarningLabel.Visible = false;

            if (Session["Logged"] == null || (bool)Session["Logged"] == false)
            {
                Response.Redirect("All.aspx");
            }
            UsersClass current_user = UsersClass.GetByID(Convert.ToInt32(Session["CurrentUserID"]));
            if (!current_user.IsMod)
            {
                Response.Redirect("All.aspx");
            }

            FillConsultPagesDDL();

            string pageid = Request.Form["PageID"];

            if (pageid != null && pageid != "")
            {
                ViewState["PageID"] = pageid;
            }

            if ((pageid == null || pageid == "") && (ViewState["PageID"] == null || ViewState["PageID"].ToString() == ""))
            {
                pageid = ConsultPagesDDL.Items[0].Value;
                ViewState["PageID"] = pageid;
            }
            else if (ViewState["PageID"] != null && ViewState["PageID"].ToString() != "")
            {
                pageid = ViewState["PageID"].ToString();
                ConsultPagesDDL.SelectedValue = pageid.ToString();
            }

            FillUsersInPageGV(Convert.ToInt32(pageid));
            FillUsersNotInPageGV(Convert.ToInt32(pageid));
        }
    }

    private void FillUsersInPageGV(int PageID)
    {

        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);
        DataTable users_dt;

        if (page == null)
        {
            WarningLabel.Text = "invalid page";
            WarningLabel.Visible = true;
            return;
        }

        if (ViewState["UsersInPage_dt"] == null)
        {
            users_dt = page.GetUsersInPage();
        }
        else
        {
            users_dt = (DataTable)ViewState["UsersInPage_dt"];
        }


        UsersInPageGV.DataSource = users_dt;
        UsersInPageGV.DataBind();
        UsersInPageGV.HeaderRow.TableSection = TableRowSection.TableHeader;

    }

    private void FillUsersNotInPageGV(int PageID)
    {

        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);
        DataTable users_dt;

        if (page == null)
        {
            WarningLabel.Text = "invalid page";
            WarningLabel.Visible = true;
            return;
        }

        if (ViewState["UsersNotInPage_dt"] == null)
        {
            users_dt = page.GetUsersNotInPage();
        }
        else
        {
            users_dt = (DataTable)ViewState["UsersNotInPage_dt"];
        }

        UsersNotInPageGV.DataSource = users_dt;
        UsersNotInPageGV.DataBind();
        UsersNotInPageGV.HeaderRow.TableSection = TableRowSection.TableHeader;

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