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

    public void LogOutUser()
    {
        Session["Logged"] = false;
        Session["CurrentUserID"] = null;
    }

    public void LogInUser(string username, string password)
    {
        UsersClass user = UsersClass.GetByCredentials(username, password);
        Session["Logged"] = true;
        Session["CurrentUserID"] = user.ID;
    }

    public void SignUpUser(string username, string password, DateTime DOB)
    {
        UsersClass.CreateNew(username, password, DOB);
        LogInUser(username, password);
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
        if(!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password) && UsersClass.UserExists(username, password))
            LogInUser(username, password);
    }

    protected void SignupSubmitButton_Click(object sender, EventArgs e)
    {
        string username = signupInputUsername.Text;
        string password = signupInputPassword.Text;
        DateTime DOB = DateTime.ParseExact(signupInputDOB.Text, "dd/mm/yyyy", null);

        if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password) && !UsersClass.UserNameTaken(username) && DOB.CompareTo(DateTime.Today) <= 0)
            SignUpUser(username, password, DOB);

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
        LogOutUser();
    }
    #endregion
}
