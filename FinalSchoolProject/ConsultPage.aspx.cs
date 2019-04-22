using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ConsultPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            object post_id = Request.QueryString["id"];
            if (post_id == null) Response.Redirect("Home.aspx");
            else
            {
                int id = Convert.ToInt32(post_id);
                //BindPostViewRepeaterRepeater(id);
                Response.Redirect("Home.aspx");

            }
        }
    }
}