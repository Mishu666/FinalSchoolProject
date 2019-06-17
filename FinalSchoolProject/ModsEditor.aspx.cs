using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class ModsEditor : System.Web.UI.Page
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
            if (!current_user.IsAdmin)
            {
                Response.Redirect("All.aspx");
            }

            FillConsultPagesDDL();

            string pageid = Request.Params["PageID"];

            if (!string.IsNullOrEmpty(pageid))
            {
                Session["PageID"] = pageid;
            }
            else
            {
                if (Session["PageID"] == null || Session["PageID"].ToString() == "")
                {
                    pageid = ConsultPagesDDL.Items[0].Value;
                    Session["PageID"] = pageid;
                }
                else
                {
                    pageid = Session["PageID"].ToString();
                    ConsultPagesDDL.SelectedValue = pageid.ToString();
                }
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

        if (ViewState["Mods_dt"] == null)
        {
            users_dt = page.GetPageMods();
        }
        else
        {
            users_dt = (DataTable)ViewState["Mods_dt"];
        }

        PageModsGV.DataSource = users_dt;
        PageModsGV.DataBind();
        PageModsGV.HeaderRow.TableSection = TableRowSection.TableHeader;

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

        if (ViewState["NotMods_dt"] == null)
        {
            users_dt = page.GetPageNotMods();
        }
        else
        {
            users_dt = (DataTable)ViewState["NotMods_dt"];
        }

        NotModsGV.DataSource = users_dt;
        NotModsGV.DataBind();
        NotModsGV.HeaderRow.TableSection = TableRowSection.TableHeader;

    }

    private void FillConsultPagesDDL()
    {
        ConsultPagesDDL.Items.Clear();

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
}