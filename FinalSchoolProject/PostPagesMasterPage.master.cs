using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PostPagesMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void BindPostRepeater(DataTable dt)
    {
        PostsRepeater.DataSource = dt;
        PostsRepeater.DataBind();
    }

    protected void PostsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        
    }

}
