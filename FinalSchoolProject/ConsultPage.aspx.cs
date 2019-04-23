﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ConsultPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            object page_id_obj = Request.QueryString["page-id"];
            if (page_id_obj == null) Response.Redirect("Home.aspx");
            else
            {
                int page_id = Convert.ToInt32(page_id_obj);
                ViewState["PageID"] = page_id;
                DataTable page_dt = PostsClass.GetByProperties(new KeyValuePair<string, object>("ConsultPageID", page_id));

                (Master as PostPagesMasterPage).BindPostRepeater(page_dt);

            }
        }
    }
}