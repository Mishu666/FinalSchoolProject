﻿using System;
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
            BindHomePostRepeater();
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
    }

    public void BindHomePostRepeater()
    {
        UsersClass user = UsersClass.GetByID((int)Session["CurrentUserID"]);
        DataTable allpostsdt = user.GetUserSubscribedPosts();
        HomePostRepeater.DataSource = allpostsdt;
        HomePostRepeater.DataBind();

    }
}