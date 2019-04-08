using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindRepeater();
    }

    private void BindRepeater()
    {
        DataTable dt = PostsClass.GetAll();
        HomePostsRepeater.DataSource = dt;
        HomePostsRepeater.DataBind();
    }


}