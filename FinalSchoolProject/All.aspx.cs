﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class All : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if(!IsPostBack)
        {
            BindHomePostRepeater();
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
    }

    public void BindHomePostRepeater()
    {
        DataTable allpostsdt = PostsClass.GetAll();
        HomePostRepeater.DataSource = allpostsdt;
        HomePostRepeater.DataBind();

    }


}