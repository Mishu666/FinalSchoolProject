using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["Logged"] == null || (bool)Session["Logged"] == false)
                Response.Redirect("All.aspx");
            BindHomePostRepeater();
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
    }

    public void BindHomePostRepeater()
    {
        UsersClass user = UsersClass.GetByID(Convert.ToInt32(Session["CurrentUserID"]));
        DataTable allpostsdt = user.GetPostsInSubscibedPages();
        HomePostRepeater.DataSource = allpostsdt;
        HomePostRepeater.DataBind();

    }
}