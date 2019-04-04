using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;

public partial class MasterPage : System.Web.UI.MasterPage
{
    #region page events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
    }

    #endregion

    #region utility methods

    public void LogInUser(string username, string password)
    {
        DataRow user = UsersClass.GetByCredentials(username, password);
        Session["Logged"] = true;
        Session["current_user_id"] = Convert.ToInt32(user["ID"]);
    }

    public void SignUpUser(string username, string password, string Email, DateTime DOB)
    {

    }

    #endregion

    #region button click events

    protected void AdminToolsImageButton_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void AccountImageButton_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void SearchImageButton_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void LoginSubmitButton_Click(object sender, EventArgs e)
    {

    }

    protected void SignupSubmitButton_Click(object sender, EventArgs e)
    {

    }

    protected void LoginCancelButton_Click(object sender, EventArgs e)
    {
        string username = loginInputUsername.Text;
        string password = loginInputPassword.Text;
        LogInUser(username, password);
    }

    protected void SwitchToSignupFormLinkButton_Click(object sender, EventArgs e)
    {

    }

    protected void SignupCancelButton_Click(object sender, EventArgs e)
    {

    }

    protected void SwitchToLoginFormLinkButton_Click(object sender, EventArgs e)
    {

    }

    #endregion
    
}
