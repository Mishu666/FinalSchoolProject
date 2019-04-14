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

    #region control events

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
        string username = loginInputUsername.Text;
        string password = loginInputPassword.Text;

        UsersService us = new UsersService();
        us.ValidateAndLogin(username, password);
    }

    protected void SignupSubmitButton_Click(object sender, EventArgs e)
    {
        string username = signupInputUsername.Text;
        string password = signupInputPassword.Text;
        DateTime DOB = DateTime.ParseExact(signupInputDOB.Text, "dd/mm/yyyy", null);

        UsersService us = new UsersService();
        us.ValidateAndSignup(username, password, DOB);

    }

    protected void LoginCancelButton_Click(object sender, EventArgs e)
    {
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

    protected void LogoutConfirmButton_Click(object sender, EventArgs e)
    {
        UsersService us = new UsersService();
        us.LogOutUser();
    }
    #endregion
}
